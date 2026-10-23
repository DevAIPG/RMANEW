using Amphenol.RMA.AccesoDatos.Data;
using Amphenol.RMA.Models;
using Amphenol.RMA.Models.Enumerations;
using Microsoft.Extensions.Configuration;
using Xunit;

public class RmaApprovalRoutingTests
{
    [Theory]
    [InlineData("QualityDirector", null)]
    [InlineData("GeneralManager", "invalid")]
    [InlineData("QualityManagerMesa", "0")]
    [InlineData("QualityManagerNogales", "-1")]
    [InlineData("QualityManagerEndicott", " ")]
    [InlineData("CustomerServiceManager", "0")]
    public void InvalidApproverSettingsFailBeforeDatabaseAccess(string section, string id)
    {
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(
            new Dictionary<string, string> { [$"{section}:ResId"] = id }).Build();
        var routing = new RmaApprovalRouting(null, configuration);
        var error = Assert.Throws<InvalidOperationException>(() => routing.GetConfiguredApprover(section));
        Assert.Contains($"{section}:ResId", error.Message);
    }

    [Fact]
    public void MissingConfiguredDirectorDoesNotFallBackToHrRolesOrChangeAssignment()
    {
        var routing = new RmaApprovalRouting(null, new ConfigurationBuilder().Build());
        var request = new CSEXSW_Rma { Totalrmavalues = 5000, Approver = "Original", res_id_approver = 42 };
        Assert.Throws<InvalidOperationException>(() => routing.Assign(request, false, true));
        Assert.Equal("Original", request.Approver);
        Assert.Equal(42, request.res_id_approver);
    }

    [Fact]
    public void SmallScrapAllowanceStillAutoApprovesWithoutConfiguredApprovers()
    {
        var routing = new RmaApprovalRouting(null, new ConfigurationBuilder().Build());
        var request = new CSEXSW_Rma
        {
            Totalrmavalues = 4999, RMA500 = "No", Rmatypeofrequest = "DISTY SCRAP ALLOWANCE"
        };
        var approver = routing.Assign(request, false, true);
        Assert.Equal(-4, approver.Id);
        Assert.Equal(-4, request.res_id_approver);
        Assert.Equal(RmaRequestStatus.AutoApprove.ToDisplayString(), request.Status);
    }

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
    [InlineData(4999.99, "Mesa", "QualityManagerMesa")]
    [InlineData(4999.99, "Nogales", "QualityManagerNogales")]
    [InlineData(4999.99, "Endicott", "QualityManagerEndicott")]
    [InlineData(5000, "Mesa", "QualityDirector")]
    [InlineData(19999.99, "Mesa", "QualityDirector")]
    [InlineData(20000, "Mesa", "QualityDirector")]
    [InlineData(20000.01, "Mesa", "QualityDirector")]
    public void NormalRoutingCoversAmountBoundariesAndSites(double amount, string site, string expectedSection)
    {
        var request = new CSEXSW_Rma { Totalrmavalues = amount, Wherebuilt = site };
        Assert.Equal(expectedSection, RmaApprovalRouting.NormalApproverSection(request));
    }

    [Theory]
    [InlineData(4999, "No", null)]
    [InlineData(4999, "Yes", "QualityDirector")]
    [InlineData(5000, "No", "QualityDirector")]
    [InlineData(20000, "No", "QualityDirector")]
    public void ScrapAllowanceKeepsItsNormalApprovalRules(double amount, string overThreshold, string section)
    {
        var request = new CSEXSW_Rma { Totalrmavalues = amount, RMA500 = overThreshold,
            Rmatypeofrequest = RmaRequestType.DistyScrapAllowance.ToDisplayString() };
        Assert.Equal(section, RmaApprovalRouting.NormalApproverSection(request));
    }

    [Fact]
    public void NormalRoutingCanUseTheExistingCurrencyAdjustedValue()
    {
        var request = new CSEXSW_Rma { Totalrmavalues = 20000, Wherebuilt = "Mesa" };
        Assert.Equal("QualityManagerMesa", RmaApprovalRouting.NormalApproverSection(request, 2800));
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
