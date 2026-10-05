using Amphenol.RMA.ViewModels;
using System.ComponentModel.DataAnnotations;
using Xunit;

public class RmaManualLineTests
{
    [Fact]
    public void ManualLineAcceptsMissingInvoiceAndSequenceAndStoresZeros()
    {
        var line = ValidLine();
        line.NoInvoice = true;
        line.InvoiceNumber = null;
        line.SequenceNumber = null;
        Assert.Empty(Validate(line));
        Assert.Equal("0", line.StoredInvoiceNumber);
        Assert.Equal((short)0, line.GetStoredSequenceNumber());
    }

    [Fact]
    public void ManualModeCannotPersistStaleInvoiceReferences()
    {
        var line = ValidLine();
        line.NoInvoice = true;
        Assert.Empty(Validate(line));
        Assert.Equal("0", line.StoredInvoiceNumber);
        Assert.Equal((short)0, line.GetStoredSequenceNumber());
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData("0", 0)]
    [InlineData("123456", 32768)]
    public void InvoiceLinkedLinesRequireUsableInvoiceReferences(string invoice, int? sequence)
    {
        var line = ValidLine();
        line.InvoiceNumber = invoice;
        line.SequenceNumber = sequence;
        Assert.NotEmpty(Validate(line));
    }

    [Fact]
    public void InvoiceLinkedLinePreservesInvoiceAndSequence()
    {
        var line = ValidLine();
        Assert.Empty(Validate(line));
        Assert.Equal("123456", line.StoredInvoiceNumber);
        Assert.Equal((short)3, line.GetStoredSequenceNumber());
    }

    [Fact]
    public void ManualLineStillRequiresPartReturnCodeAndAmounts()
    {
        var line = ValidLine();
        line.NoInvoice = true;
        line.PartNumber = "";
        line.ReturnCode = "";
        line.AuthorizedQuantity = 0;
        line.Price = 0;
        line.UnitCost = 0;
        var errors = Validate(line).SelectMany(x => x.MemberNames).ToArray();
        Assert.Contains(nameof(line.PartNumber), errors);
        Assert.Contains(nameof(line.ReturnCode), errors);
        Assert.Contains(nameof(line.AuthorizedQuantity), errors);
        Assert.Contains(nameof(line.Price), errors);
        Assert.Contains(nameof(line.UnitCost), errors);
    }

    private static RmaLineViewModel ValidLine() => new()
    {
        RmaRequestId = 1, InvoiceNumber = "123456", SequenceNumber = 3, PartNumber = "PART001",
        SelectedAction = "C", SelectedLocation = "MRM", AuthorizedQuantity = 2,
        Price = 10, UnitCost = 5, ReturnCode = "001"
    };

    private static List<ValidationResult> Validate(RmaLineViewModel line)
    {
        var errors = new List<ValidationResult>();
        Validator.TryValidateObject(line, new ValidationContext(line), errors, true);
        return errors;
    }
}
