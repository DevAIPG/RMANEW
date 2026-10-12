using System;

namespace Amphenol.RMA.Models.Enumerations
{
    public enum RmaSubmitStatus
    {
        NotSubmitted,
        Submitted,
        CsReviewRequired
    }
    public static class RmaSubmitStatusExtensions
    {
        public static string ToDisplayString(this RmaSubmitStatus status)
        {
            return status switch
            {
                RmaSubmitStatus.NotSubmitted => "Not Submitted",
                RmaSubmitStatus.Submitted => "Submitted",
                RmaSubmitStatus.CsReviewRequired => "CS Review Required",
                _ => status.ToString()
            };
        }
        public static RmaSubmitStatus FromDisplayString( string value)
        {
            return value switch
            {
                "Not Submitted" => RmaSubmitStatus.NotSubmitted,
                "Submitted" => RmaSubmitStatus.Submitted,
                "CS Review Required" => RmaSubmitStatus.CsReviewRequired,
                _ => throw new ArgumentException(
                    $"Unknown RMA submit status: {value}",
                    nameof(value))
            };
        }
    }
}
