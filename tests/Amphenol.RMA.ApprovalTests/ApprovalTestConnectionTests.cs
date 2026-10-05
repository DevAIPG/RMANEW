using Xunit;

public class ApprovalTestConnectionTests
{
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
