using Amphenol.RMA.AccesoDatos.Data;
using Xunit;

public class ErpFieldValidatorTests
{
    [Theory]
    [InlineData("001")]
    [InlineData("001 ")]
    [InlineData("001     ")]
    [InlineData(" A ")]
    [InlineData("    ")]
    [InlineData("")]
    [InlineData(null)]
    public void AcceptsCodesThatFitIgnoringOnlyTrailingSpacePadding(string value)
    {
        ErpFieldValidator.ValidateTextLength("mfg_loc", value, 3);
    }

    [Theory]
    [InlineData("0001")]
    [InlineData("0001 ")]
    [InlineData(" 001")]
    [InlineData("001\t")]
    [InlineData("001\u00a0")]
    public void RejectsOverlongContentIncludingLeadingSpacesAndOtherWhitespace(string value)
    {
        var error = Assert.Throws<InvalidOperationException>(() =>
            ErpFieldValidator.ValidateTextLength("mfg_loc", value, 3));
        Assert.Contains("mfg_loc exceeds 3", error.Message);
    }

    [Fact]
    public void PaddedCustomerNamesRemainUnchangedAndOverlongNamesAreRejected()
    {
        var value = "Customer Industrial Operations".PadRight(50);
        var original = value;
        ErpFieldValidator.ValidateTextLength("ship_to_name", value, 40);
        Assert.Equal(original, value);
        Assert.Equal(50, value.Length);

        Assert.Throws<InvalidOperationException>(() =>
            ErpFieldValidator.ValidateTextLength("ship_to_name", new string('A', 41), 40));
    }
}
