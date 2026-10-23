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

        public static string NormalApproverSection(CSEXSW_Rma rma, double? approvalValue = null)
        {
            var total = approvalValue ?? rma.Totalrmavalues;
            if (rma.Rmatypeofrequest == RmaRequestType.DistyScrapAllowance.ToDisplayString())
                return total < 5000 && rma.RMA500 == "No" ? null : "QualityDirector";
            if (total >= 5000) return "QualityDirector";
            return rma.Wherebuilt switch
            {
                "Nogales" => "QualityManagerNogales",
                "Mesa" => "QualityManagerMesa",
                "Endicott" => "QualityManagerEndicott",
                _ => throw new InvalidOperationException("No quality manager is configured for this location.")
            };
        }

        public Approver GetConfiguredApprover(string section)
        {
            if (!int.TryParse(_configuration[$"{section}:ResId"], out var id) || id <= 0)
                throw new InvalidOperationException($"{section}:ResId must identify an existing employee.");

            var employee = _db.humres.FirstOrDefault(x => x.res_id == id);
            if (employee == null)
                throw new InvalidOperationException($"{section}:ResId must identify an existing employee.");

            var configuredEmail = _configuration[$"{section}:Email"];
            return new Approver
            {
                Id = employee.res_id,
                Name = employee.fullname,
                Email = string.IsNullOrWhiteSpace(configuredEmail) ? employee.mail : configuredEmail.Trim()
            };
        }

        // Sumbit identifies the CS step; normal routing is recalculated after its approval.
        public Approver Assign(CSEXSW_Rma rma, bool requiresCsReview, bool submitted, double? approvalValue = null)
        {
            Approver approver;
            if (submitted && requiresCsReview)
            {
                approver = GetConfiguredApprover("CustomerServiceManager");
                rma.Sumbit = RmaSubmitStatus.CsReviewRequired.ToDisplayString();
                rma.Status = RmaRequestStatus.Pending.ToDisplayString();
            }
            else
            {
                var section = NormalApproverSection(rma, approvalValue);
                if (section == null)
                    approver = new Approver { Id = -4, Name = "System" };
                else
                {
                    approver = GetConfiguredApprover(section);
                }
                rma.Sumbit = (submitted ? RmaSubmitStatus.Submitted : RmaSubmitStatus.NotSubmitted).ToDisplayString();
                if (submitted)
                    rma.Status = (section == null ? RmaRequestStatus.AutoApprove : RmaRequestStatus.Pending).ToDisplayString();
            }
            rma.Approver = approver.Name;
            rma.res_id_approver = approver.Id;
            return approver;
        }
    }
}
