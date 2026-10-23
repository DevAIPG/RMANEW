using Amphenol.RMA.Models.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Amphenol.RMA.Models
{
    public static class RmaWithdrawalPolicy
    {
        public static bool CanCancelAnyRequest(string username, string employeeId,
            int customerServiceManagerId, IEnumerable<string> approvalAdministrators,
            int customerServiceSupervisorId = 0)
        {
            if (string.IsNullOrWhiteSpace(username)) return false;

            return (customerServiceManagerId > 0
                    && int.TryParse(employeeId, out var id) && id == customerServiceManagerId)
                || (approvalAdministrators?.Any(user => !string.IsNullOrWhiteSpace(user)
                    && string.Equals(user.Trim(), username.Trim(), StringComparison.OrdinalIgnoreCase)) ?? false)
                || (customerServiceSupervisorId > 0
                    && int.TryParse(employeeId, out var supervisorId) && supervisorId == customerServiceSupervisorId);
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
