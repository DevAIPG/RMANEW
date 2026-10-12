using System;
using System.Collections.Generic;
using System.Linq;
using Amphenol.RMA.Models;
using Amphenol.RMA.Models.Enumerations;
using Amphenol.RMA.Models.ModelsM10;
using Microsoft.Extensions.Configuration;

namespace Amphenol.RMA.AccesoDatos.Data
{
    public sealed class RmaApprovalRouting
    {
        private readonly DbContextM10 _db;
        private readonly IConfiguration _configuration;

        public RmaApprovalRouting(DbContextM10 db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }

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

        public static int? NormalApprovalRole(CSEXSW_Rma rma, double? approvalValue = null)
        {
            var total = approvalValue ?? rma.Totalrmavalues;
            if (rma.Rmatypeofrequest == RmaRequestType.DistyScrapAllowance.ToDisplayString())
                return total < 5000 && rma.RMA500 == "No" ? null : 100031;
            if (total >= 5000) return 100031;
            return rma.Wherebuilt switch
            {
                "Nogales" => 100030,
                "Mesa" => 100062,
                "Endicott" => 100039,
                _ => throw new InvalidOperationException("No quality manager is configured for this location.")
            };
        }

        // Sumbit identifies the CS step; normal routing is recalculated after its approval.
        public Approver Assign(CSEXSW_Rma rma, bool requiresCsReview, bool submitted, double? approvalValue = null)
        {
            Approver approver;
            if (submitted && requiresCsReview)
            {
                int.TryParse(_configuration["CustomerServiceManager:ResId"], out var id);
                var employee = id > 0 ? _db.humres.FirstOrDefault(x => x.res_id == id) : null;
                if (employee == null)
                    throw new InvalidOperationException("CustomerServiceManager:ResId must identify an existing employee.");
                approver = new Approver { Id = employee.res_id, Name = employee.fullname,
                    Email = _configuration["CustomerServiceManager:Email"] ?? employee.mail };
                rma.Sumbit = RmaSubmitStatus.CsReviewRequired.ToDisplayString();
                rma.Status = RmaRequestStatus.Pending.ToDisplayString();
            }
            else
            {
                var roleId = NormalApprovalRole(rma, approvalValue);
                if (roleId == null)
                    approver = new Approver { Id = -4, Name = "System" };
                else
                {
                    var employee = (from role in _db.HRRoles
                                    join person in _db.humres on role.EmpID equals person.res_id
                                    where role.RoleID == roleId.Value
                                    select person).FirstOrDefault();
                    if (employee == null)
                        throw new InvalidOperationException($"No employee is assigned to approval role {roleId}.");
                    approver = new Approver { Id = employee.res_id, Name = employee.fullname, Email = employee.mail };
                }
                rma.Sumbit = (submitted ? RmaSubmitStatus.Submitted : RmaSubmitStatus.NotSubmitted).ToDisplayString();
                if (submitted)
                    rma.Status = (roleId == null ? RmaRequestStatus.AutoApprove : RmaRequestStatus.Pending).ToDisplayString();
            }
            rma.Approver = approver.Name;
            rma.res_id_approver = approver.Id;
            return approver;
        }
    }
}
