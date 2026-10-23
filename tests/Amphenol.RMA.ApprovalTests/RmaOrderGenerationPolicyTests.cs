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

    [Theory]
    [InlineData("supervisor", "303", 303, true)]
    [InlineData("supervisor", " 303 ", 303, true)]
    [InlineData("supervisor", "303", 0, false)]
    [InlineData("other", "404", 303, false)]
    [InlineData("supervisor", null, 303, false)]
    [InlineData("supervisor", "invalid", 303, false)]
    [InlineData(null, "303", 303, false)]
    [InlineData(" ", "303", 303, false)]
    [InlineData("supervisor", "0", 0, false)]
    [InlineData("supervisor", "-1", -1, false)]
    public void SupervisorCanGenerateOrdersAndCancelOthersRequestsOnlyWhenConfigured(
        string username, string employeeId, int supervisorId, bool expected)
    {
        Assert.Equal(expected, RmaOrderGenerationPolicy.CanGenerateOrder(
            101, username, employeeId, 202, Array.Empty<string>(), supervisorId));
        Assert.Equal(expected, RmaWithdrawalPolicy.CanCancelAnyRequest(
            username, employeeId, 202, Array.Empty<string>(), supervisorId));
    }

    [Fact]
    public void SupervisorCancellationStillRequiresAnEligibleRequest()
    {
        Assert.True(RmaWithdrawalPolicy.CanCancelAnyRequest(
            "supervisor", "303", 202, Array.Empty<string>(), 303));
        Assert.True(RmaWithdrawalPolicy.CanCancel("Pending", null));
        Assert.False(RmaWithdrawalPolicy.CanCancel("Approved", null));
        Assert.False(RmaWithdrawalPolicy.CanCancel("Pending", "123456"));
    }
}
