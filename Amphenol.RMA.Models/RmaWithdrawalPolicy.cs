using Amphenol.RMA.Models.Enumerations;
using System;

namespace Amphenol.RMA.Models
{
    public static class RmaWithdrawalPolicy
    {
        public static bool CanCancelAnyRequest(string username, string employeeId,
            int customerServiceManagerId, bool hasItBypassRole,
            int customerServiceSupervisorId = 0)
        {
            if (string.IsNullOrWhiteSpace(username)) return false;

            var hasEmployeeId = int.TryParse(employeeId, out var id) && id > 0;
            return (customerServiceManagerId > 0
                    && hasEmployeeId && id == customerServiceManagerId)
                || (hasEmployeeId && hasItBypassRole)
                || (customerServiceSupervisorId > 0
                    && hasEmployeeId && id == customerServiceSupervisorId);
        }

        public static bool CanWithdraw(string status, string submission, string rmaNumber)
        {
            if (!string.IsNullOrWhiteSpace(rmaNumber)) return false;

            return string.Equals(status?.Trim(), RmaRequestStatus.Pending.ToDisplayString(), StringComparison.OrdinalIgnoreCase)
                && (string.Equals(submission?.Trim(), RmaSubmitStatus.Submitted.ToDisplayString(), StringComparison.OrdinalIgnoreCase)
                    || string.Equals(submission?.Trim(), RmaSubmitStatus.CsReviewRequired.ToDisplayString(), StringComparison.OrdinalIgnoreCase));
        }

        public static bool CanCancel(string status, string rmaNumber)
        {
            if (!string.IsNullOrWhiteSpace(rmaNumber)) return false;

            return string.Equals(status?.Trim(), RmaRequestStatus.Pending.ToDisplayString(), StringComparison.OrdinalIgnoreCase)
                || string.Equals(status?.Trim(), RmaRequestStatus.Remark.ToDisplayString(), StringComparison.OrdinalIgnoreCase)
                || string.Equals(status?.Trim(), RmaRequestStatus.Rejected.ToDisplayString(), StringComparison.OrdinalIgnoreCase);
        }
    }
}
