using Amphenol.RMA.Models.Enumerations;
using System;

namespace Amphenol.RMA.Models
{
    public static class RmaEditPolicy
    {
        public static bool CanEdit(string status, string submission, string rmaNumber)
        {
            if (!string.IsNullOrWhiteSpace(rmaNumber)) return false;
            bool IsStatus(RmaRequestStatus value) => string.Equals(
                status?.Trim(), value.ToDisplayString(), StringComparison.OrdinalIgnoreCase);

            if (IsStatus(RmaRequestStatus.Remark) || IsStatus(RmaRequestStatus.Rejected)) return true;
            return IsStatus(RmaRequestStatus.Pending) && string.Equals(
                submission?.Trim(), RmaSubmitStatus.NotSubmitted.ToDisplayString(), StringComparison.OrdinalIgnoreCase);
        }
    }
}
