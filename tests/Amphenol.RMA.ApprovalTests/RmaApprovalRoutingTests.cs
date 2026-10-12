using Amphenol.RMA.AccesoDatos.Data;
using Amphenol.RMA.Models;
using Amphenol.RMA.Models.Enumerations;
using Xunit;

public class RmaApprovalRoutingTests
{
    [Theory]
    [InlineData("123456", 4, 5, false)]
    [InlineData("0", 4, 5, true)]
    [InlineData(null, 4, 5, true)]
    [InlineData(" 0 ", 0, 5, true)]
    [InlineData("0", 5, 5, false)]
    [InlineData("0", 6, 5, false)]
    [InlineData("0", 0, 0, false)]
    public void CsReviewRequiresBothMissingInvoiceAndBelowCostPrice(string invoice, int price, int cost, bool expected)
    {
        var lines = new[]
        {
            new csexsw_coustumer { Invoice = "123456", Unit = 1, Cost = 100 },
            new csexsw_coustumer { Invoice = invoice, Unit = price, Cost = cost }
        };
        Assert.Equal(expected, RmaApprovalRouting.RequiresCsReview(lines));
    }

    [Theory]
    [InlineData(4999.99, "Mesa", 100062)]
    [InlineData(4999.99, "Nogales", 100030)]
    [InlineData(4999.99, "Endicott", 100039)]
    [InlineData(5000, "Mesa", 100031)]
    [InlineData(19999.99, "Mesa", 100031)]
    [InlineData(20000, "Mesa", 100031)]
    [InlineData(20000.01, "Mesa", 100031)]
    public void NormalRoutingCoversAmountBoundariesAndSites(double amount, string site, int expectedRole)
    {
        var request = new CSEXSW_Rma { Totalrmavalues = amount, Wherebuilt = site };
        Assert.Equal(expectedRole, RmaApprovalRouting.NormalApprovalRole(request));
    }

    [Theory]
    [InlineData(4999, "No", null)]
    [InlineData(4999, "Yes", 100031)]
    [InlineData(5000, "No", 100031)]
    [InlineData(20000, "No", 100031)]
    public void ScrapAllowanceKeepsItsNormalApprovalRules(double amount, string overThreshold, int? role)
    {
        var request = new CSEXSW_Rma { Totalrmavalues = amount, RMA500 = overThreshold,
            Rmatypeofrequest = RmaRequestType.DistyScrapAllowance.ToDisplayString() };
        Assert.Equal(role, RmaApprovalRouting.NormalApprovalRole(request));
    }

    [Fact]
    public void NormalRoutingCanUseTheExistingCurrencyAdjustedValue()
    {
        var request = new CSEXSW_Rma { Totalrmavalues = 20000, Wherebuilt = "Mesa" };
        Assert.Equal(100062, RmaApprovalRouting.NormalApprovalRole(request, 2800));
        Assert.Equal(20000d, request.Totalrmavalues);
    }

    [Fact]
    public void CsSubmissionStatusFitsExistingColumnAndRoundTrips()
    {
        var label = RmaSubmitStatus.CsReviewRequired.ToDisplayString();
        Assert.True(label.Length <= 20);
        Assert.Equal(RmaSubmitStatus.CsReviewRequired, RmaSubmitStatusExtensions.FromDisplayString(label));
    }
}
