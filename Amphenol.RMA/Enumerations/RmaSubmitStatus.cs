using System;

namespace Amphenol.RMA.Enumerations
{
    public enum RmaSubmitStatus
    {
        NotSubmitted,
        Submitted
    }
    public static class RmaSubmitStatusExtensions
    {
        public static string ToDisplayString(this RmaSubmitStatus status)
        {
            return status switch
            {
                RmaSubmitStatus.NotSubmitted => "Not Submitted",
                RmaSubmitStatus.Submitted => "Submitted",
                _ => status.ToString()
            };
        }
        public static RmaSubmitStatus FromDisplayString( string value)
        {
            return value switch
            {
                "Not Submitted" => RmaSubmitStatus.NotSubmitted,
                "Submitted" => RmaSubmitStatus.Submitted,
                _ => throw new ArgumentException(
                    $"Unknown RMA submit status: {value}",
                    nameof(value))
            };
        }
    }
}
