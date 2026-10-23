using Amphenol.RMA.Models;
using Xunit;

public class RmaOrderGenerationPolicyTests
{
    [Theory]
    [InlineData(101, "requester", "101", true)]
    [InlineData(101, "manager", "202", true)]
    [InlineData(101, " ADMIN ", "303", true)]
    [InlineData(101, "admin", null, true)]
    [InlineData(101, "other", "303", false)]
    [InlineData(101, "other", null, false)]
    [InlineData(101, "other", "invalid", false)]
    [InlineData(101, null, "101", false)]
    [InlineData(0, "other", "0", false)]
    public void OnlyRequesterManagerAndConfiguredBypassUsersCanGenerateOrders(
        int requesterId, string username, string employeeId, bool expected)
    {
        Assert.Equal(expected, RmaOrderGenerationPolicy.CanGenerateOrder(
            requesterId, username, employeeId, 202, new[] { "admin" }));
    }

    [Fact]
    public void MissingConfigurationDoesNotGrantAccess()
    {
        Assert.False(RmaOrderGenerationPolicy.CanGenerateOrder(0, "other", "0", 0, null));
        Assert.False(RmaOrderGenerationPolicy.CanGenerateOrder(101, "other", "303", 0, null));
        Assert.True(RmaOrderGenerationPolicy.CanGenerateOrder(101, "requester", "101", 0, null));
    }
}
