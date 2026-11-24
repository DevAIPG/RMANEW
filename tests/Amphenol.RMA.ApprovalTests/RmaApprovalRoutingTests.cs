using Amphenol.RMA.AccesoDatos.Data;
using Amphenol.RMA.Models;
using Amphenol.RMA.Models.Constants;
using Amphenol.RMA.Models.Enumerations;
using Amphenol.RMA.ViewModels;
using Xunit;

public class RmaApprovalRoutingTests
{
    private readonly RmaApprovalRouting _routing = new(null);

    [Theory]
    [InlineData("123456", false)]
    [InlineData("0", true)]
    [InlineData(" 0 ", true)]
    [InlineData(null, true)]
    [InlineData("   ", true)]
    public void CsReviewIsRequiredWhenAnyLineHasNoInvoice(string invoice, bool expected)
    {
        var lines = new[]
        {
            new csexsw_coustumer { Invoice = "123456", Unit = 1, Cost = 100 },
            new csexsw_coustumer { Invoice = invoice, Unit = 10, Cost = 5 }
        };

        Assert.Equal(expected, RmaApprovalRouting.RequiresCsReview(lines));
    }

    [Fact]
    public void InvoiceLinkedLinesDoNotRequireCsReviewEvenWhenBelowCost()
    {
        var lines = new[] { new csexsw_coustumer { Invoice = "123456", Unit = 4, Cost = 5 } };

        Assert.False(RmaApprovalRouting.RequiresCsReview(lines));
    }

    [Theory]
    [InlineData(4999.99, null)]
    [InlineData(5000, RmaRoles.PlantManager)]
    [InlineData(19999.99, RmaRoles.PlantManager)]
    [InlineData(20000, RmaRoles.BusinessUnitDirector)]
    [InlineData(49999.99, RmaRoles.BusinessUnitDirector)]
    [InlineData(50000, RmaRoles.GeneralManager)]
    [InlineData(90000, RmaRoles.GeneralManager)]
    public void StandardRequestsRouteThroughTheFlowchartAmountBands(double amount, string? nextRole)
    {
        var request = new CSEXSW_Rma { Totalrmavalues = amount, Wherebuilt = "Mesa", Rmatypeofrequest = "CREDIT ONLY" };

        Assert.Equal(RmaRoles.QualityManager, _routing.NormalApproverRole(request));
        Assert.Equal(nextRole, _routing.NextApprovalRole(request));
    }

    [Theory]
    [InlineData(4999, "No", true, null)]
    [InlineData(4999, "Yes", false, RmaRoles.DistributionManager)]
    [InlineData(4999, "", false, null)]
    [InlineData(5000, "No", false, RmaRoles.DistributionManager)]
    [InlineData(19999.99, "No", false, RmaRoles.DistributionManager)]
    [InlineData(20000, "No", false, RmaRoles.BusinessUnitDirector)]
    [InlineData(49999.99, "No", false, RmaRoles.BusinessUnitDirector)]
    [InlineData(50000, "No", false, RmaRoles.GeneralManager)]
    public void ScrapAllowanceUsesItsAutoApprovalAndRoleBands(
        double amount, string lineAtLeast500, bool autoApprove, string? nextRole)
    {
        var request = new CSEXSW_Rma
        {
            Totalrmavalues = amount,
            RMA500 = lineAtLeast500,
            Rmatypeofrequest = RmaRequestType.DistyScrapAllowance.ToDisplayString()
        };

        Assert.Equal(autoApprove, RmaApprovalRouting.IsAutoApprovedScrapAllowance(request));
        Assert.Equal(autoApprove ? null : RmaRoles.QualityManager, _routing.NormalApproverRole(request));
        Assert.Equal(nextRole, _routing.NextApprovalRole(request));
    }

    [Fact]
    public void ScrapItemThresholdIsInclusiveAtFiveHundredDollars()
    {
        var request = new CSEXSW_Rma
        {
            Totalrmavalues = 4999,
            RMA500 = "Yes",
            Rmatypeofrequest = RmaRequestType.DistyScrapAllowance.ToDisplayString()
        };

        Assert.False(RmaApprovalRouting.IsAutoApprovedScrapAllowance(request));
        Assert.Equal(RmaRoles.QualityManager, _routing.NormalApproverRole(request));
        Assert.Equal(RmaRoles.DistributionManager, _routing.NextApprovalRole(request));
    }

    [Fact]
    public void ScrapAllowanceWithFiveHundredDollarUnitPriceIsNotAutoApproved()
    {
        var viewModel = new RmaViewModel
        {
            Lines = [new RmaLineViewModel { AuthorizedQuantity = 1, Price = 500m }]
        };
        var request = new CSEXSW_Rma
        {
            Totalrmavalues = (double)viewModel.TotalValue,
            RMA500 = viewModel.HasLineOverThreshold ? "Yes" : "No",
            Rmatypeofrequest = RmaRequestType.DistyScrapAllowance.ToDisplayString()
        };

        Assert.Equal(500m, viewModel.TotalValue);
        Assert.Equal("Yes", request.RMA500);
        Assert.False(RmaApprovalRouting.IsAutoApprovedScrapAllowance(request));
        Assert.Equal(RmaRoles.DistributionManager, _routing.NextApprovalRole(request));
    }

    [Theory]
    [InlineData(1, 499.99, false)]
    [InlineData(1, 500, true)]
    [InlineData(500, 10, false)]
    [InlineData(500, 499.99, false)]
    public void ViewModelFlagsLinesByUnitPriceThreshold(
        int quantity, decimal unitPrice, bool expected)
    {
        var request = new RmaViewModel
        {
            Lines = [new RmaLineViewModel { AuthorizedQuantity = quantity, Price = unitPrice }]
        };

        Assert.Equal(expected, request.HasLineOverThreshold);
    }

    [Fact]
    public void RoutingUsesCurrencyAdjustedApprovalValueWithoutChangingStoredTotal()
    {
        var request = new CSEXSW_Rma { Totalrmavalues = 20000, Wherebuilt = "Mesa", Rmatypeofrequest = "CREDIT ONLY" };

        Assert.Null(_routing.NextApprovalRole(request, approvalValue: 2800));
        Assert.Equal(RmaRoles.PlantManager, _routing.NextApprovalRole(request, approvalValue: 5000));
        Assert.Equal(20000d, request.Totalrmavalues);
    }

    [Theory]
    [InlineData("CREDIT ONLY", 4999, "PlantManager|CustomerServiceManager|Controller")]
    [InlineData("CREDIT ONLY", 5000, "PlantManager|CustomerServiceManager|Controller")]
    [InlineData("CREDIT ONLY", 20000, "PlantManager|CustomerServiceManager|Controller|BusinessUnitDirector")]
    [InlineData("CREDIT ONLY", 50000, "PlantManager|CustomerServiceManager|Controller|SupplyChainDirector|BusinessUnitDirector")]
    [InlineData("DISTY SCRAP ALLOWANCE", 4999, "PlantManager|CustomerServiceManager|Controller|DistributionManager")]
    [InlineData("DISTY SCRAP ALLOWANCE", 5000, "PlantManager|CustomerServiceManager|Controller|SupplyChainDirector")]
    [InlineData("DISTY SCRAP ALLOWANCE", 20000, "PlantManager|CustomerServiceManager|Controller")]
    [InlineData("DISTY SCRAP ALLOWANCE", 50000, "PlantManager|CustomerServiceManager|Controller|SupplyChainDirector|BusinessUnitDirector")]
    public void CompletionNotificationRolesMatchTheFlowchart(string requestType, double amount, string expectedRoles)
    {
        var request = new CSEXSW_Rma { Rmatypeofrequest = requestType, Totalrmavalues = amount };

        Assert.Equal(expectedRoles.Split('|'), RmaApprovalRouting.CompletionNotificationRoles(request));
    }

    [Fact]
    public void CsSubmissionStatusFitsExistingColumnAndRoundTrips()
    {
        var label = RmaSubmitStatus.CsReviewRequired.ToDisplayString();

        Assert.True(label.Length <= 20);
        Assert.Equal(RmaSubmitStatus.CsReviewRequired, RmaSubmitStatusExtensions.FromDisplayString(label));
    }
}
