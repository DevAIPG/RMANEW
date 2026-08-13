namespace Amphenol.RMA.Models.Enumerations
{
    public enum RmaRequestType
    {
        ValueAdd,
        CreditOnly,
        DistyScrapAllowance
    }
    public static class RmaRequestTypeExtensions
    {
        public static string ToDisplayString(this RmaRequestType requestType)
        {
            return requestType switch
            {
                RmaRequestType.ValueAdd => "VALUE ADD RMA",
                RmaRequestType.CreditOnly => "CREDIT ONLY",
                RmaRequestType.DistyScrapAllowance => "DISTY SCRAP ALLOWANCE",
                _ => requestType.ToString()
            };
        }
    }
}
