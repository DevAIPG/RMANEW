using Amphenol.RMA.ViewModels;
using System.ComponentModel.DataAnnotations;
using Xunit;

public class RmaBuildLocationTests
{
    [Theory]
    [InlineData("Nogales")]
    [InlineData("Endicott")]
    [InlineData("Mesa")]
    public void SupportedLocationSurvivesLookupReload(string location)
    {
        var vm = new RmaViewModel { SelectedBuildLocation = location };

        // Both POST actions rebuild lookups when another field fails validation.
        vm.InitializeLookups();

        Assert.Equal(location, vm.SelectedBuildLocation);
        Assert.Contains(vm.SelectedBuildLocation, vm.BuildLocations);
        Assert.Empty(ValidateLocation(vm));
    }

    [Theory]
    [InlineData("Mesa    ", "Mesa")]
    [InlineData("Endicott ", "Endicott")]
    [InlineData(" Nogales ", "Nogales")]
    public void StoredPaddingDoesNotPreventDropdownSelection(string stored, string expected)
    {
        var vm = new RmaViewModel { SelectedBuildLocation = stored };

        Assert.Equal(expected, vm.SelectedBuildLocation);
        Assert.Contains(vm.SelectedBuildLocation, vm.BuildLocations);
        Assert.Empty(ValidateLocation(vm));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("Unknown")]
    [InlineData("Endicot")]
    public void MissingOrUnsupportedLocationCannotBeSaved(string location)
    {
        var vm = new RmaViewModel { SelectedBuildLocation = location };

        Assert.NotEmpty(ValidateLocation(vm));
        Assert.NotEqual("Nogales", vm.SelectedBuildLocation);
    }

    [Fact]
    public void NewRequestRequiresAnExplicitLocation()
    {
        var vm = new RmaViewModel();

        Assert.Null(vm.SelectedBuildLocation);
        Assert.NotEmpty(ValidateLocation(vm));
    }

    private static List<ValidationResult> ValidateLocation(RmaViewModel vm)
    {
        var errors = new List<ValidationResult>();
        Validator.TryValidateProperty(vm.SelectedBuildLocation,
            new ValidationContext(vm) { MemberName = nameof(vm.SelectedBuildLocation) }, errors);
        return errors;
    }
}
