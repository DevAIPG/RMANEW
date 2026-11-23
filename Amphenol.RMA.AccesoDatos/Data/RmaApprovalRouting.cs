using Amphenol.RMA.Models;
using Amphenol.RMA.Models.Constants;
using Amphenol.RMA.Models.Enumerations;
using Amphenol.RMA.Models.ModelsM10;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Amphenol.RMA.AccesoDatos.Data
{
    public sealed class RmaApprovalRouting(DbContextM10 db)
    {
        public static bool RequiresCsReview(IEnumerable<csexsw_coustumer> lines) =>
            lines.Any(x => string.IsNullOrWhiteSpace(x.Invoice) || x.Invoice.Trim() == "0");

        public static double ApprovalValue(CSEXSW_Rma rma, DbContext500 erp)
        {
            var customer = erp.arcusfil_sql
                .FirstOrDefault(x =>
                    x.cus_no.Trim() == rma.Customer.Trim());

            if (customer is null)
            {
                throw new InvalidOperationException(
                    $"Customer '{rma.Customer}' was not found.");
            }

            if (!string.Equals(
                    customer.curr_cd,
                    "CNY",
                    StringComparison.OrdinalIgnoreCase))
            {
                return Convert.ToDouble(rma.Totalrmavalues);
            }

            var latestRate = erp.Rate
                .Where(x => x.SourceCurrency == "USD")
                .OrderByDescending(x => x.DateL)
                .FirstOrDefault();

            if (latestRate is null || latestRate.RateExchange == 0)
            {
                throw new InvalidOperationException(
                    "A valid USD exchange rate could not be found.");
            }

            return Convert.ToDouble(rma.Totalrmavalues) / Convert.ToDouble(latestRate.RateExchange);
        }

        public string? NormalApproverRole(CSEXSW_Rma rma, double? approvalValue = null)
        {
            var total = approvalValue ?? rma.Totalrmavalues;
            if (IsAutoApprovedScrapAllowance(rma, total))
            {
                return null;
            }

            return RmaRoles.QualityManager;
        }

        public static bool IsAutoApprovedScrapAllowance(CSEXSW_Rma rma, double? approvalValue = null) =>
            rma.Rmatypeofrequest == RmaRequestType.DistyScrapAllowance.ToDisplayString()
            && (approvalValue ?? rma.Totalrmavalues) < 5000
            && string.Equals(rma.RMA500, "No", StringComparison.OrdinalIgnoreCase);

        public string? NextApprovalRole(CSEXSW_Rma rma, double? approvalValue = null)
        {
            var total = approvalValue ?? rma.Totalrmavalues;
            if (total < 5000) return null;
            if (total < 20000)
                return rma.Rmatypeofrequest == RmaRequestType.DistyScrapAllowance.ToDisplayString()
                    ? RmaRoles.DistributionManager
                    : RmaRoles.PlantManager;
            if (total < 50000) return RmaRoles.BusinessUnitDirector;
            return RmaRoles.GeneralManager;
        }

        public static IReadOnlyList<string> CompletionNotificationRoles(CSEXSW_Rma rma, double? approvalValue = null)
        {
            var total = approvalValue ?? rma.Totalrmavalues;
            var isScrapAllowance = rma.Rmatypeofrequest == RmaRequestType.DistyScrapAllowance.ToDisplayString();

            if (isScrapAllowance)
            {
                if (total < 5000)
                    return [RmaRoles.PlantManager, RmaRoles.CustomerServiceManager, RmaRoles.Controller,
                        RmaRoles.DistributionManager];
                if (total < 20000)
                    return [RmaRoles.PlantManager, RmaRoles.CustomerServiceManager, RmaRoles.Controller,
                        RmaRoles.SupplyChainDirector];
                if (total < 50000)
                    return [RmaRoles.PlantManager, RmaRoles.CustomerServiceManager, RmaRoles.Controller];
                return [RmaRoles.PlantManager, RmaRoles.CustomerServiceManager, RmaRoles.Controller,
                    RmaRoles.SupplyChainDirector, RmaRoles.BusinessUnitDirector];
            }

            if (total < 20000)
                return [RmaRoles.PlantManager, RmaRoles.CustomerServiceManager, RmaRoles.Controller];
            if (total < 50000)
                return [RmaRoles.PlantManager, RmaRoles.CustomerServiceManager, RmaRoles.Controller,
                    RmaRoles.BusinessUnitDirector];
            return [RmaRoles.PlantManager, RmaRoles.CustomerServiceManager, RmaRoles.Controller,
                RmaRoles.SupplyChainDirector, RmaRoles.BusinessUnitDirector];
        }

        public (IReadOnlyList<Approver> Recipients, IReadOnlyList<string> MissingRoles)
            ResolveCompletionNotificationApprovers(CSEXSW_Rma rma, double? approvalValue = null)
        {
            var recipients = new List<Approver>();
            var missingRoles = new List<string>();
            foreach (var role in CompletionNotificationRoles(rma, approvalValue))
            {
                try
                {
                    recipients.Add(GetApprover(role, AssignmentLocation(role, rma)));
                }
                catch (InvalidOperationException)
                {
                    missingRoles.Add(role);
                }
            }

            return (recipients, missingRoles);
        }

        private static string AssignmentLocation(string roleTitle, CSEXSW_Rma rma) =>
            roleTitle is RmaRoles.QualityManager or RmaRoles.PlantManager
                ? rma.Wherebuilt
                : "NorthAmerica";

        public Approver? GetNextApprover(CSEXSW_Rma rma, int currentApproverId, double? approvalValue = null)
        {
            var nextRole = NextApprovalRole(rma, approvalValue);
            var firstApprover = GetApprover(RmaRoles.QualityManager, rma.Wherebuilt);
            if (currentApproverId != firstApprover.Id && nextRole is null)
                throw new InvalidOperationException(
                    $"Approver '{currentApproverId}' is not part of the configured approval route for request '{rma.Rmarequest}'.");
            if (currentApproverId == firstApprover.Id && nextRole is not null)
                return GetApprover(nextRole, AssignmentLocation(nextRole, rma));
            if (nextRole is null) return null;

            var finalApprover = GetApprover(nextRole, AssignmentLocation(nextRole, rma));
            if (currentApproverId == finalApprover.Id) return null;

            throw new InvalidOperationException(
                $"Approver '{currentApproverId}' is not part of the configured approval route for request '{rma.Rmarequest}'.");
        }

        public Approver GetApproverByEmployeeNumber(int employeeNumber)
        {
            var assignment = db.RmaRoleAssignments
                .FirstOrDefault(x => x.IsActive && x.EmployeeNumber == employeeNumber);

            if (assignment == null)
                throw new InvalidOperationException($"No active RMA role assignment exists for employee '{employeeNumber}'.");

            if (assignment.EmployeeNumber is not > 0)
                throw new InvalidOperationException($"RMA role assignment for employee '{employeeNumber}' has no valid employee number.");

            return new Approver { Id = assignment.EmployeeNumber ?? -1, Name = assignment.DisplayName, Email = assignment.Email };
        }

        public bool HasActiveRoleAssignment(int employeeNumber, string roleTitle) =>
            employeeNumber > 0 && db.RmaRoleAssignments.Any(x => x.IsActive
                && x.EmployeeNumber == employeeNumber && x.RmaRole.Title == roleTitle);

        public Approver GetApprover(string roleTitle, string location = "NorthAmerica")
        {
            var assignment = db.RmaRoleAssignments
                .FirstOrDefault(x => x.IsActive && x.Location == location && x.RmaRole.Title == roleTitle);

            if (assignment == null)
            {
                throw new InvalidOperationException($"No active assignment exists for role '{roleTitle}' at location '{location}'.");
            }

            if (assignment.EmployeeNumber is not > 0)
            {
                throw new InvalidOperationException(
                    $"The active assignment for role '{roleTitle}' at location '{location}' has no valid employee number.");
            }

            return new Approver
            {
                Id = assignment.EmployeeNumber ?? -1,
                Name = assignment.DisplayName,
                Email = assignment.Email
            };
        }

        // Sumbit identifies the CS step; normal routing is recalculated after its approval.
        public Approver Assign(CSEXSW_Rma rma, bool requiresCsReview, bool submitted, double? approvalValue = null)
        {
            Approver approver;
            if (submitted && requiresCsReview)
            {
                approver = GetApprover(RmaRoles.CustomerServiceManager);
                rma.Sumbit = RmaSubmitStatus.CsReviewRequired.ToDisplayString();
                rma.Status = RmaRequestStatus.Pending.ToDisplayString();
            }
            else
            {
                var role = NormalApproverRole(rma, approvalValue);
                approver = role == null ? new Approver { Id = -4, Name = "System" } : GetApprover(role, rma.Wherebuilt);
                rma.Sumbit = (submitted ? RmaSubmitStatus.Submitted : RmaSubmitStatus.NotSubmitted).ToDisplayString();
                if (submitted)
                    rma.Status = (role == null ? RmaRequestStatus.AutoApprove : RmaRequestStatus.Pending).ToDisplayString();
            }
            rma.Approver = approver.Name;
            rma.res_id_approver = approver.Id;
            return approver;
        }
    }
}
