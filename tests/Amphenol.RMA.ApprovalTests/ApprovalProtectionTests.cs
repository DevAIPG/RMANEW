using Amphenol.RMA.AccesoDatos.Data;
using Amphenol.RMA.Controllers;
using Amphenol.RMA.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;
using System.Reflection;
using Xunit;

public class ApprovalProtectionTests
{
    [Theory]
    [InlineData("Approved", null)]
    [InlineData("approved", "")]
    [InlineData("Pending", "  601761")]
    [InlineData("Rejected", "601761")]
    public void FinalOrPartialApprovalCannotBecomeEditable(string status, string number)
    {
        Assert.Throws<InvalidOperationException>(() => RmaApprovalGuard.EnsureEditable(
            new CSEXSW_Rma { Status = status, turno = number }));
    }

    [Theory]
    [InlineData("Pending")]
    [InlineData("Rejected")]
    public void RequestsWithoutApprovalRemainEditable(string status)
    {
        RmaApprovalGuard.EnsureEditable(new CSEXSW_Rma { Status = status, Sumbit = "Not Submitted", turno = " " });
    }

    [Theory]
    [InlineData("Pending", "Submitted")]
    [InlineData(" pending ", " submitted ")]
    [InlineData("Auto-Approved", "Submitted")]
    [InlineData("Auto-Approved", "Not Submitted")]
    [InlineData("Approved", "Not Submitted")]
    [InlineData("Pending", null)]
    public void WaitingOrFinalRequestsCannotBeEditedEvenWithoutAnErpNumber(string status, string submission)
    {
        var request = new CSEXSW_Rma { Status = status, Sumbit = submission };
        Assert.False(request.CanEdit);
        Assert.Throws<InvalidOperationException>(() => RmaApprovalGuard.EnsureEditable(request));
    }

    [Theory]
    [InlineData("Remark")]
    [InlineData("Rejected")]
    public void ReturnedSubmittedRequestsCanBeEditedUntilResubmitted(string status)
    {
        var request = new CSEXSW_Rma { Status = status, Sumbit = "Submitted" };
        Assert.True(request.CanEdit);
        RmaApprovalGuard.EnsureEditable(request);
        request.Status = "Pending";
        Assert.False(request.CanEdit);
        Assert.Throws<InvalidOperationException>(() => RmaApprovalGuard.EnsureEditable(request));
    }

    [Fact]
    public void ListAndEntityAgreeAboutEditingAtTheGmStage()
    {
        var entity = new CSEXSW_Rma { Status = "Pending", Sumbit = "Submitted", res_id_approver = 42 };
        var row = new Amphenol.RMA.Models.ModelsM10.CSEXSW_Rma_ViewModel
        { Status = entity.Status, Sumbit = entity.Sumbit, turno = entity.turno };
        Assert.False(entity.CanEdit);
        Assert.False(row.CanEdit);
    }

    [Fact]
    public void PublicApprovalActionCannotSelectTheSystemIdentity()
    {
        var action = typeof(RmaController).GetMethod("Aprobar", BindingFlags.Public | BindingFlags.Instance);
        Assert.NotNull(action);
        Assert.Equal(new[] { "commentt", "idsa" }, action.GetParameters().Select(x => x.Name));
        Assert.Null(typeof(RmaController).GetMethod("ApproveAutomatically", BindingFlags.Public | BindingFlags.Instance));
        Assert.Null(typeof(RmaController).GetMethod("CompleteApproval", BindingFlags.Public | BindingFlags.Instance));
    }

    [Fact]
    public void LegacyCreationAndRecoveryFailWithoutOpeningSqlConnections()
    {
        var connections = new ForbidConnection();
        using var erp = new DbContext500(new DbContextOptionsBuilder<DbContext500>()
            .UseSqlServer("Server=unused;Database=unused;Integrated Security=true")
            .AddInterceptors(connections).Options);
        var repository = new OERDTFIL_SQLRepository(erp, null, null);
        Assert.Throws<InvalidOperationException>(() => repository.nuevorma());
        Assert.Throws<InvalidOperationException>(() => repository.lineascrear(
            null, 1, "601761", null, null, null, null, null, null, null, null, null, null));
        Assert.Throws<InvalidOperationException>(() => repository.lineas(
            null, 1, "601761", null, null, null, null, null, null, null, null, null, null));
        Assert.Throws<InvalidOperationException>(() =>
        {
            _ = repository.falloRMA(
                null, 1, "601761", null, null, null, null, null, null, null, null, null, null);
        });
        Assert.Equal(0, connections.Attempts);
        var job = typeof(OERDTFIL_SQLRepository).GetMethod("falloRMA");
        Assert.Equal(0, job.GetCustomAttribute<Hangfire.AutomaticRetryAttribute>().Attempts);
    }

    private sealed class ForbidConnection : DbConnectionInterceptor
    {
        public int Attempts { get; private set; }
        public override InterceptionResult ConnectionOpening(DbConnection connection,
            ConnectionEventData eventData, InterceptionResult result)
        {
            Attempts++;
            throw new Exception("This test must not access a database.");
        }
    }
}
