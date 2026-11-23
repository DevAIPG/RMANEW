using Amphenol.RMA.Models;
using Xunit;

public class RmaOrderGenerationPolicyTests
{
    [Theory]
    [InlineData(101, "requester", "101", false, true)]
    [InlineData(101, "manager", "202", false, true)]
    [InlineData(101, "it.user", "303", true, true)]
    [InlineData(101, "other", "303", false, false)]
    [InlineData(101, "other", "303", true, true)]
    [InlineData(101, "other", null, false, false)]
    [InlineData(101, "other", null, true, false)]
    [InlineData(101, "other", "invalid", false, false)]
    [InlineData(101, null, "101", true, false)]
    [InlineData(0, "other", "0", false, false)]
    public void OnlyRequesterManagerAndItRoleCanGenerateOrders(
        int requesterId, string username, string employeeId, bool hasItRole, bool expected)
    {
        Assert.Equal(expected, RmaOrderGenerationPolicy.CanGenerateOrder(
            requesterId, username, employeeId, 202, hasItRole));
    }

    [Fact]
    public void MissingItRoleDoesNotGrantAccess()
    {
        Assert.False(RmaOrderGenerationPolicy.CanGenerateOrder(0, "other", "0", 0, false));
        Assert.False(RmaOrderGenerationPolicy.CanGenerateOrder(101, "other", "303", 0, false));
        Assert.True(RmaOrderGenerationPolicy.CanGenerateOrder(101, "requester", "101", 0, false));
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
            101, username, employeeId, 202, false, supervisorId));
        Assert.Equal(expected, RmaWithdrawalPolicy.CanCancelAnyRequest(
            username, employeeId, 202, false, supervisorId));
    }

    [Fact]
    public void SupervisorCancellationStillRequiresAnEligibleRequest()
    {
        Assert.True(RmaWithdrawalPolicy.CanCancelAnyRequest(
            "supervisor", "303", 202, false, 303));
        Assert.True(RmaWithdrawalPolicy.CanCancel("Pending", null));
        Assert.False(RmaWithdrawalPolicy.CanCancel("Approved", null));
        Assert.False(RmaWithdrawalPolicy.CanCancel("Pending", "123456"));
    }
}
