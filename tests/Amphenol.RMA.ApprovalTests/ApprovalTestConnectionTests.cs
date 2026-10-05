using Xunit;
using Microsoft.Extensions.Configuration;

public class ApprovalTestConnectionTests
{
    [Fact]
    public void ReadsTheSameConnectionNamesAsTheApplication()
    {
        var m10 = "Server=M10TestNA01;Database=M10;Integrated Security=True;";
        var erp = "Server=AIO-POS;Database=500;Integrated Security=True;";
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string>
        {
            ["ConnectionStrings:ConnectionM10"] = m10,
            ["ConnectionStrings:Connection500"] = erp
        }).Build();
        Assert.Equal((m10, erp), ApprovalTestConnection.ReadConfiguredConnections(configuration));
    }

    [Theory]
    [InlineData("PRODUCTION", "AIO-POS")]
    [InlineData("M10TestNA01", "PRODUCTION")]
    public void EitherUnapprovedDestinationRejectsTheEntireConnectionPair(string m10Server, string erpServer)
    {
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string>
        {
            ["ConnectionStrings:ConnectionM10"] = $"Server={m10Server};Database=M10;Integrated Security=True;",
            ["ConnectionStrings:Connection500"] = $"Server={erpServer};Database=500;Integrated Security=True;"
        }).Build();
        Assert.Throws<InvalidOperationException>(() => ApprovalTestConnection.ReadConfiguredConnections(configuration));
    }

    [Fact]
    public void MissingApplicationSettingsFailInsteadOfSkipping()
    {
        var error = Assert.Throws<InvalidOperationException>(() =>
            ApprovalTestConnection.ReadConfiguredConnections(new ConfigurationBuilder().Build()));
        Assert.Contains("ConnectionM10", error.Message);
    }

    [Theory]
    [InlineData("AIO-POS")]
    [InlineData("M10TestNA01")]
    [InlineData("aio-pos")]
    [InlineData("m10testna01")]
    [InlineData("tcp:AIO-POS,1433")]
    [InlineData("M10TestNA01\\SQLTEST")]
    public void ApprovedTestServersKeepTheirExistingDatabaseNames(string server)
    {
        var connection = $"Server={server};Database=500;Integrated Security=True;";
        Assert.Equal(connection, ApprovalTestConnection.Validate(connection));
    }

    [Theory]
    [InlineData("PRODUCTION")]
    [InlineData("localhost")]
    [InlineData("127.0.0.1")]
    [InlineData("AIO-POS-PROD")]
    [InlineData("M10TestNA01.other-domain")]
    [InlineData("tcp:PRODUCTION,1433")]
    public void UnapprovedServersAreRejectedEvenWithTheOldTestDatabaseSuffix(string server)
    {
        Assert.Throws<InvalidOperationException>(() => ApprovalTestConnection.Validate(
            $"Server={server};Database=500_RmaApprovalTests;Integrated Security=True;"));
    }

    [Fact]
    public void ConnectionMustSpecifyADatabase()
    {
        Assert.Throws<InvalidOperationException>(() => ApprovalTestConnection.Validate(
            "Server=AIO-POS;Integrated Security=True;"));
    }
}
