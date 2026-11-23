using Amphenol.RMA.Models;
using Amphenol.RMA.Models.Constants;
using Amphenol.RMA.Models.Enumerations;
using Amphenol.RMA.Models.ModelsM10;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Amphenol.RMA.AccesoDatos.Data
{
    public sealed class RmaApprovalRouting(DbContextM10 db)
    {
        public static bool RequiresCsReview(IEnumerable<csexsw_coustumer> lines) =>
            lines.Any(x => (string.IsNullOrWhiteSpace(x.Invoice) || x.Invoice.Trim() == "0")
                && x.Unit < x.Cost);

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
            if (rma.Rmatypeofrequest == RmaRequestType.DistyScrapAllowance.ToDisplayString() && total < 5000 && rma.RMA500 == "No")
            {
                return null;
            }

            return db.RmaRoleAssignments.Any(x => x.RmaRole.Title == RmaRoles.QualityManager && x.Location == rma.Wherebuilt && x.IsActive)
                ? RmaRoles.QualityManager
                : RmaRoles.PlantManager;
        }

        public Approver GetApprover(string roleTitle, string location = "NorthAmerica")
        {
            var assignment = db.RmaRoleAssignments
                .FirstOrDefault(x => x.IsActive && x.Location == location && x.RmaRole.Title == roleTitle);

            if (assignment == null)
            {
                throw new InvalidOperationException($"No active assignment exists for role '{roleTitle}' at location '{location}'.");
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
