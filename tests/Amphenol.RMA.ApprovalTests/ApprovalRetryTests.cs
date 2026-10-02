using Amphenol.RMA.AccesoDatos.Data;
using Amphenol.RMA.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Xunit;

// These integration tests intentionally require isolated SQL Server clones.
// Fixtures are consumed, so restore the clones before repeating the suite.
[CollectionDefinition("Approval SQL", DisableParallelization = true)]
public class ApprovalSqlCollection { }

public sealed class ApprovalSqlFactAttribute : FactAttribute
{
    public ApprovalSqlFactAttribute()
    {
        if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("RMA_TEST_M10"))
            || string.IsNullOrEmpty(Environment.GetEnvironmentVariable("RMA_TEST_ERP")))
            Skip = "Set RMA_TEST_M10 and RMA_TEST_ERP to isolated SQL test clones.";
    }
}

[Collection("Approval SQL")]
public class ApprovalRetryTests
{
    [ApprovalSqlFact]
    public void ErpFailureRollsBackBothDatabasesAndCounter()
    {
        var id = FixtureId("RMA_TEST_ROLLBACK_ID");
        using var m10 = M10();
        using var erp = Erp(new FailAfterErpSave());
        var request = PendingRequest(m10, id);
        var userId = request.res_id_approver.ToString();
        var originalStatus = request.Status;
        var before = Snapshot(erp);

        Assert.Throws<InjectedFailure>(() => Repository(m10, erp)
            .Updateaprobar(id, "Rollback test", userId));
        erp.ChangeTracker.Clear();
        m10.ChangeTracker.Clear();
        Assert.Equal(before, Snapshot(erp));
        var failed = m10.CSEXSW_Rma.Single(x => x.Id == id);
        Assert.Equal(originalStatus, failed.Status);
        Assert.True(string.IsNullOrWhiteSpace(failed.turno));

        using var retryM10 = M10();
        using var retryErp = Erp();
        Assert.Equal("Approved", Repository(retryM10, retryErp)
            .Updateaprobar(id, "Retry", userId));
        AssertCompleted(retryM10, retryErp, id);
        Assert.Equal(int.Parse(before.Item1) + 1, int.Parse(Counter(retryErp)));
    }

    [ApprovalSqlFact]
    public void M10SaveFailureLeavesErpUntouched()
    {
        var id = FixtureId("RMA_TEST_M10_FAILURE_ID");
        using var m10 = M10(new FailM10Save());
        var erpSaves = new CountErpSaves();
        using var erp = Erp(erpSaves);
        var request = PendingRequest(m10, id);
        var before = Snapshot(erp);
        var originalStatus = request.Status;
        Assert.Throws<InjectedFailure>(() => Repository(m10, erp)
            .Updateaprobar(id, "Failure test", request.res_id_approver.ToString()));
        erp.ChangeTracker.Clear();
        m10.ChangeTracker.Clear();
        Assert.Equal(0, erpSaves.Count);
        Assert.Equal(before, Snapshot(erp));
        var failed = m10.CSEXSW_Rma.Single(x => x.Id == id);
        Assert.Equal(originalStatus, failed.Status);
        Assert.True(string.IsNullOrWhiteSpace(failed.turno));
    }

    [ApprovalSqlFact]
    public void InvalidLaterLinePreventsM10AndErpWrites()
    {
        var id = FixtureId("RMA_TEST_VALIDATION_FAILURE_ID");
        using var m10 = M10();
        var erpSaves = new CountErpSaves();
        using var erp = Erp(erpSaves);
        var request = PendingRequest(m10, id);
        var line = m10.csexsw_coustumer.Where(x => x.RmaId == id).OrderByDescending(x => x.Id).First();
        var originalItem = line.Coustumer;
        line.Coustumer = "INVALID-TEST-ITEM";
        m10.SaveChanges();
        var before = Snapshot(erp);
        var originalStatus = request.Status;
        try
        {
            Assert.Throws<InvalidOperationException>(() => Repository(m10, erp)
                .Updateaprobar(id, "Invalid item test", request.res_id_approver.ToString()));
            erp.ChangeTracker.Clear();
            m10.ChangeTracker.Clear();
            Assert.Equal(0, erpSaves.Count);
            Assert.Equal(before, Snapshot(erp));
            Assert.Equal(originalStatus, m10.CSEXSW_Rma.Single(x => x.Id == id).Status);
        }
        finally
        {
            m10.ChangeTracker.Clear();
            var restore = m10.csexsw_coustumer.Single(x => x.Id == line.Id);
            restore.Coustumer = originalItem;
            m10.SaveChanges();
        }
    }

    [ApprovalSqlFact]
    public async Task ConcurrentApprovalsCreateOneRmaAndOneApprovalResult()
    {
        var id = FixtureId("RMA_TEST_CONCURRENT_ID");
        string userId;
        int originalCounter;
        using (var m10 = M10())
        using (var erp = Erp())
        {
            userId = PendingRequest(m10, id).res_id_approver.ToString();
            originalCounter = int.Parse(Counter(erp));
        }

        using var ready = new CountdownEvent(2);
        using var start = new ManualResetEventSlim();
        string Approve()
        {
            using var m10 = M10();
            using var erp = Erp();
            // Match the controller's pre-lock read to exercise stale tracking.
            m10.CSEXSW_Rma.Single(x => x.Id == id);
            ready.Signal();
            if (!start.Wait(TimeSpan.FromSeconds(15))) throw new TimeoutException();
            return Repository(m10, erp).Updateaprobar(id, "Concurrent test", userId);
        }

        var first = Task.Run(Approve);
        var second = Task.Run(Approve);
        var bothReady = ready.Wait(TimeSpan.FromSeconds(15));
        start.Set();
        Assert.True(bothReady);
        var results = await Task.WhenAll(first, second);
        Assert.Single(results, x => x == "Approved");
        Assert.Single(results, x => x == "AlreadyApproved");
        using var finalM10 = M10();
        using var finalErp = Erp();
        AssertCompleted(finalM10, finalErp, id);
        Assert.Equal(originalCounter + 1, int.Parse(Counter(finalErp)));
    }

    private static CSEXSW_Rma PendingRequest(DbContextM10 m10, int id)
    {
        var request = m10.CSEXSW_Rma.Single(x => x.Id == id);
        Assert.NotEqual("Approved", request.Status);
        Assert.True(string.IsNullOrWhiteSpace(request.turno));
        // Use fixtures below the two-stage threshold with valid ERP master data.
        Assert.InRange(request.Totalrmavalues, 0, 19999);
        Assert.True(request.res_id_approver > 0);
        Assert.True(m10.csexsw_coustumer.Any(x => x.RmaId == id));
        return request;
    }

    private static void AssertCompleted(DbContextM10 m10, DbContext500 erp, int id)
    {
        var request = m10.CSEXSW_Rma.AsNoTracking().Single(x => x.Id == id);
        Assert.Equal("Approved", request.Status);
        Assert.Equal(1, erp.OERHDFIL_SQL.Count(x => x.rma_no == request.turno));
        Assert.Equal(m10.csexsw_coustumer.Count(x => x.RmaId == id),
            erp.OERDTFIL_SQL.Count(x => x.rma_no == request.turno));
    }

    private static string Counter(DbContext500 erp) =>
        erp.OERMACTL_SQL.AsNoTracking().Single(x => x.ID == 1).ctl_next_order_no;

    private static (string, int, int, int) Snapshot(DbContext500 erp) =>
        (Counter(erp), erp.OERHDFIL_SQL.Count(), erp.OERDTFIL_SQL.Count(), erp.iminvloc_sql.Count());

    private static int FixtureId(string name) => int.Parse(
        Environment.GetEnvironmentVariable(name)
        ?? throw new InvalidOperationException($"Set {name} to a fresh pending request ID."));

    private static string TestConnection(string name)
    {
        var value = Environment.GetEnvironmentVariable(name)
            ?? throw new InvalidOperationException($"Set {name}.");
        var builder = new Microsoft.Data.SqlClient.SqlConnectionStringBuilder(value);
        if (!builder.InitialCatalog.EndsWith("_RmaApprovalTests", StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("Test database names must end with _RmaApprovalTests.");
        return value;
    }

    private static DbContextM10 M10(SaveChangesInterceptor interceptor = null)
    {
        var options = new DbContextOptionsBuilder<DbContextM10>()
            .UseSqlServer(TestConnection("RMA_TEST_M10"));
        if (interceptor != null) options.AddInterceptors(interceptor);
        return new DbContextM10(options.Options);
    }

    private static DbContext500 Erp(SaveChangesInterceptor interceptor = null)
    {
        var options = new DbContextOptionsBuilder<DbContext500>()
            .UseSqlServer(TestConnection("RMA_TEST_ERP"));
        if (interceptor != null) options.AddInterceptors(interceptor);
        return new DbContext500(options.Options);
    }

    private static CSEXSW_RmaRepository Repository(DbContextM10 m10, DbContext500 erp) =>
        new(m10, erp, new ConfigurationBuilder().Build());

    private sealed class InjectedFailure : Exception { }

    private sealed class FailM10Save : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData data,
            InterceptionResult<int> result)
        {
            if (data.Context.ChangeTracker.Entries<CSEXSW_Rma>()
                .Any(x => x.State == EntityState.Modified && x.Entity.Status == "Approved"))
                throw new InjectedFailure();
            return result;
        }
    }

    private sealed class CountErpSaves : SaveChangesInterceptor
    {
        public int Count { get; private set; }
        public override InterceptionResult<int> SavingChanges(DbContextEventData data,
            InterceptionResult<int> result)
        {
            Count++;
            return result;
        }
    }

    private sealed class FailAfterErpSave : SaveChangesInterceptor
    {
        // Fail after SQL executed every ERP insert, but before coordinated commit.
        public override int SavedChanges(SaveChangesCompletedEventData data, int result)
            => throw new InjectedFailure();
    }
}
