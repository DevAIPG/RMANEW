using Amphenol.RMA.Models.Enumerations;
using Amphenol.RMA.Models.Enumerations;
using System;

namespace Amphenol.RMA.Models
{
    public static class RmaWithdrawalPolicy
    {
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
