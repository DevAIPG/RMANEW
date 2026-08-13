using System;

namespace Amphenol.RMA.Models.Enumerations
{
    public enum RmaRequestStatus
    {
        Pending,
        Remark,
        Rejected,
        Approved,
        AutoApprove
    }
    public static class RmaRequestStatusExtensions
    {
        public static string ToDisplayString(this RmaRequestStatus status)
        {
            return status switch
            {
                RmaRequestStatus.Pending => "Pending",
                RmaRequestStatus.Approved => "Approved",
                RmaRequestStatus.AutoApprove => "Auto-Approved",
                _ => status.ToString()
            };
        }
        public static RmaRequestStatus FromDisplayString(string value)
        {
            return value switch
            {
                "Pending" => RmaRequestStatus.Pending,
                "Remark" => RmaRequestStatus.Remark,
                "Rejected" => RmaRequestStatus.Remark,
                "Approved" => RmaRequestStatus.Approved,
                "Auto-Approved" => RmaRequestStatus.AutoApprove,
                _ => throw new ArgumentException(
                    $"Unknown RMA request status: {value}",
                    nameof(value))
            };
        }
    }
}
