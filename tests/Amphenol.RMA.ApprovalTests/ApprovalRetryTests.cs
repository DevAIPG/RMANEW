using Amphenol.RMA.AccesoDatos.Data;
using Amphenol.RMA.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Xunit;
using Xunit.Abstractions;

// These integration tests intentionally require isolated SQL Server clones.
// Automatic selection uses distinct eligible requests; successful approvals consume them.
[CollectionDefinition("Approval SQL", DisableParallelization = true)]
public class ApprovalSqlCollection { }

[Collection("Approval SQL")]
public class ApprovalRetryTests
{
    private static readonly ApprovalFixtureSelector Fixtures = new(FindFixtureCandidates, Environment.GetEnvironmentVariable);
    private readonly ITestOutputHelper _output;

    public ApprovalRetryTests(ITestOutputHelper output)
    {
        _output = output;
        // Log destinations only, never credentials or full connection strings.
        var m10 = new Microsoft.Data.SqlClient.SqlConnectionStringBuilder(ApprovalTestConfiguration.M10);
        var erp = new Microsoft.Data.SqlClient.SqlConnectionStringBuilder(ApprovalTestConfiguration.ERP);
        output.WriteLine($"M10 test destination: {m10.DataSource} / {m10.InitialCatalog}");
        output.WriteLine($"ERP test destination: {erp.DataSource} / {erp.InitialCatalog}");
    }

    [Fact]
    public void ReturnedRequestCanBeEditedAndResubmissionLocksHeaderLinesAndAttachments()
    {
        var id = FixtureId("RMA_TEST_EDIT_LIFECYCLE_ID");
        using var m10 = M10();
        using var erp = Erp();
        var request = PendingRequest(m10, id);
        Assert.Equal("Submitted", request.Sumbit);
        var repository = Repository(m10, erp);
        var children = new csexsw_coustumerRepository(m10, new ConfigurationBuilder().Build());
        var before = Snapshot(erp);
        var linesBefore = m10.csexsw_coustumer.Count(x => x.RmaId == id);
        var filesBefore = m10.CSEXSW_Attachmentrma.Count(x => x.RmaId == id);
        Assert.Throws<InvalidOperationException>(() => repository.Update(request));
        Assert.Throws<InvalidOperationException>(() => children.archivos("blocked.txt", id));
        Assert.Throws<InvalidOperationException>(() => children.lineas(
            request, null, null, null, null, null, id, null, null, null, null, false, null));

        repository.UpdateRema(id, "Return for changes");
        m10.Entry(request).Reload();
        Assert.True(request.CanEdit);
        Assert.Equal(string.Empty, repository.Updateaprobar(id, "Not resubmitted", request.res_id_approver.ToString()));
        // Exercise legacy callers that pass an already tracked request instance.
        request.Contact = "Updated after return";
        repository.Update(request);
        Assert.Equal("Updated after return", m10.CSEXSW_Rma.AsNoTracking().Single(x => x.Id == id).Contact);

        request.Status = "Pending";
        request.Sumbit = "Not Submitted";
        m10.SaveChanges();
        Assert.Equal(string.Empty, repository.Updateaprobar(id, "Draft approval blocked", request.res_id_approver.ToString()));
        request.Sumbit = "Submitted";
        m10.SaveChanges();
        Assert.Throws<InvalidOperationException>(() => repository.Update(request));
        Assert.Throws<InvalidOperationException>(() => children.archivos("blocked-after-resubmit.txt", id));
        Assert.Equal(linesBefore, m10.csexsw_coustumer.Count(x => x.RmaId == id));
        Assert.Equal(filesBefore, m10.CSEXSW_Attachmentrma.Count(x => x.RmaId == id));
        Assert.Equal(before, Snapshot(erp));
    }

    [Fact]
    public void StaleLegacyEditCannotEraseACommittedApprovalOrCreateAnotherRma()
    {
        var id = FixtureId("RMA_TEST_EDIT_PROTECTION_ID");
        using var staleM10 = M10();
        using var staleErp = Erp();
        var staleRequest = PendingRequest(staleM10, id);
        var userId = staleRequest.res_id_approver.ToString();
        var staleRepository = Repository(staleM10, staleErp);
        var reset = new CSEXSW_Rma { Id = id, Status = "Pending", turno = "" };

        using var approvalM10 = M10();
        using var approvalErp = Erp();
        var before = Snapshot(approvalErp);
        // A submitted request is protected before approval too.
        Assert.Throws<InvalidOperationException>(() => staleRepository.Update(reset));
        Assert.Equal(before, Snapshot(approvalErp));
        Assert.Equal("Approved", Repository(approvalM10, approvalErp).Updateaprobar(id, "Approve", userId));
        var committed = Snapshot(approvalErp);
        var number = approvalM10.CSEXSW_Rma.AsNoTracking().Single(x => x.Id == id).turno;

        Assert.Throws<InvalidOperationException>(() => staleRepository.Update(reset));
        var protectedRequest = staleM10.CSEXSW_Rma.AsNoTracking().Single(x => x.Id == id);
        Assert.Equal("Approved", protectedRequest.Status);
        Assert.Equal(number, protectedRequest.turno);
        Assert.Equal("AlreadyApproved", staleRepository.Updateaprobar(id, "Retry after blocked edit", userId));
        Assert.Equal(committed, Snapshot(approvalErp));
        AssertCompleted(approvalM10, approvalErp, id);
    }

    [Fact]
    public void ConfiguredBypassUserCanApproveAnotherUsersRequestWithoutDuplicateApproval()
    {
        var id = FixtureId("RMA_TEST_BYPASS_ID");
        using var m10 = M10();
        using var erp = Erp();
        var request = PendingRequest(m10, id);
        var bypassUser = m10.humres.AsNoTracking().First(x => x.res_id > 0
            && x.res_id != request.res_id_approver && x.usr_id != null && x.usr_id.Trim() != "");
        var before = Snapshot(erp);

        // The same account must be denied when it is not on the configured list.
        Assert.Equal(string.Empty, Repository(m10, erp)
            .Updateaprobar(id, "Not configured", bypassUser.res_id.ToString()));
        Assert.Equal(before, Snapshot(erp));

        var repository = BypassRepository(m10, erp, bypassUser.usr_id);
        Assert.Equal("Approved", repository.Updateaprobar(id, "Bypass approval", bypassUser.res_id.ToString()));
        AssertCompleted(m10, erp, id);
        var committed = Snapshot(erp);
        Assert.Equal("AlreadyApproved", repository.Updateaprobar(id, "Repeated bypass", bypassUser.res_id.ToString()));
        Assert.Equal(committed, Snapshot(erp));
    }

    [Fact]
    public void ConfiguredBypassUserCanApproveBothStagesWithoutSkippingTheGmHandoff()
    {
        var id = FixtureId("RMA_TEST_BYPASS_DIRECTOR_ID");
        using var m10 = M10();
        using var erp = Erp();
        var request = RequireRequest(m10, id);
        var directorId = m10.HRRoles.Single(x => x.RoleID == 100031).EmpID;
        var generalManagerId = m10.HRRoles.Single(x => x.RoleID == 100032).EmpID;
        Assert.Equal(directorId, request.res_id_approver);
        Assert.NotEqual("Approved", request.Status);
        Assert.Equal("Pending", request.Status);
        Assert.Equal("Submitted", request.Sumbit);
        Assert.True(string.IsNullOrWhiteSpace(request.turno));
        Assert.True(request.Totalrmavalues >= 20000);
        var bypassUser = m10.humres.AsNoTracking().First(x => x.res_id > 0
            && x.res_id != directorId && x.res_id != generalManagerId
            && x.usr_id != null && x.usr_id.Trim() != "");
        var repository = BypassRepository(m10, erp, bypassUser.usr_id);
        var before = Snapshot(erp);

        Assert.Equal("Pending", repository.Updateaprobar(id, "Director stage bypass", bypassUser.res_id.ToString()));
        m10.Entry(request).Reload();
        Assert.Equal(generalManagerId, request.res_id_approver);
        Assert.Equal(before, Snapshot(erp));
        Assert.True(string.IsNullOrWhiteSpace(request.turno));

        Assert.Equal("Approved", repository.Updateaprobar(id, "GM stage bypass", bypassUser.res_id.ToString()));
        AssertCompleted(m10, erp, id);
    }

    [Fact]
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

    [Fact]
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

    [Fact]
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

    [Fact]
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
            RequireRequest(m10, id);
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
        var request = RequireRequest(m10, id);
        Assert.True(request.Status == "Pending" && request.Sumbit == "Submitted",
            $"Fixture Id={id} is not waiting for approval: Status='{request.Status}', Sumbit='{request.Sumbit}'. Select a fresh submitted pending request.");
        Assert.True(string.IsNullOrWhiteSpace(request.turno),
            $"Fixture Id={id} already has ERP RMA '{request.turno}'. Select a fresh request; successful approval tests consume their fixtures.");
        // Use fixtures below the two-stage threshold with valid ERP master data.
        Assert.True(double.IsFinite(request.Totalrmavalues) && request.Totalrmavalues >= 0 && request.Totalrmavalues < 20000,
            $"Fixture Id={id} must be a USD request below $20,000 for this test.");
        Assert.True(request.res_id_approver > 0, $"Fixture Id={id} needs an assigned employee approver.");
        Assert.True(m10.csexsw_coustumer.Any(x => x.RmaId == id), $"Fixture Id={id} has no request lines.");
        return request;
    }

    private static CSEXSW_Rma RequireRequest(DbContextM10 m10, int id)
    {
        var request = m10.CSEXSW_Rma.SingleOrDefault(x => x.Id == id);
        if (request != null) return request;
        var connection = m10.Database.GetDbConnection();
        throw new InvalidOperationException(
            $"No CSEXSW_Rma row with Id={id} exists in '{connection.Database}' on '{connection.DataSource}'. " +
            "Set the fixture variable to an existing CSEXSW_Rma.Id in this test database, not the displayed Rmarequest or ERP RMA number. " +
            "If this request exists on another test server, check the ConnectionM10 setting selected by the test environment.");
    }

    private static void AssertCompleted(DbContextM10 m10, DbContext500 erp, int id)
    {
        var request = m10.CSEXSW_Rma.AsNoTracking().Single(x => x.Id == id);
        Assert.Equal("Approved", request.Status);
        Assert.Equal(1, erp.OERHDFIL_SQL.Count(x => x.rma_no == request.turno));
        Assert.Equal(m10.csexsw_coustumer.Count(x => x.RmaId == id),
            erp.OERDTFIL_SQL.Count(x => x.rma_no == request.turno));
    }

    private static string Counter(DbContext500 erp)
    {
        var counter = erp.OERMACTL_SQL.AsNoTracking().SingleOrDefault(x => x.ID == 1);
        if (counter == null)
        {
            var connection = erp.Database.GetDbConnection();
            throw new InvalidOperationException(
                $"ERP test database '{connection.Database}' on '{connection.DataSource}' has no OERMACTL_SQL row with ID=1. Check Connection500 and test ERP setup.");
        }
        return counter.ctl_next_order_no;
    }

    private static (string, int, int, int) Snapshot(DbContext500 erp) =>
        (Counter(erp), erp.OERHDFIL_SQL.Count(), erp.OERDTFIL_SQL.Count(), erp.iminvloc_sql.Count());

    private int FixtureId(string name)
    {
        var id = Fixtures.Select(name);
        _output.WriteLine($"Fixture {name}: CSEXSW_Rma.Id={id} ({(string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(name)) ? "automatically selected" : "manual override")})");
        return id;
    }

    private static IEnumerable<int> FindFixtureCandidates(string name, IReadOnlyCollection<int> excluded)
    {
        // Context creation validates BOTH connection destinations before querying.
        using var m10 = M10();
        using var erp = Erp();
        var idsToExclude = excluded.ToArray();
        var requests = m10.CSEXSW_Rma.AsNoTracking().Where(x =>
            x.Status == "Pending" && x.Sumbit == "Submitted"
            && (x.turno == null || x.turno.Trim() == "")
            && x.res_id_approver > 0
            && m10.csexsw_coustumer.Any(line => line.RmaId == x.Id)
            && m10.humres.Any(employee => employee.res_id == x.res_id_approver));
        if (idsToExclude.Length > 0)
            requests = requests.Where(x => !idsToExclude.Contains(x.Id));

        int directorId = 0, managerId = 0;
        if (name == "RMA_TEST_BYPASS_DIRECTOR_ID")
        {
            var director = m10.HRRoles.SingleOrDefault(x => x.RoleID == 100031);
            var manager = m10.HRRoles.SingleOrDefault(x => x.RoleID == 100032);
            if (director == null || manager == null || director.EmpID <= 0 || manager.EmpID <= 0
                || !m10.humres.Any(x => x.res_id == manager.EmpID))
                throw new InvalidOperationException("The director-stage test requires quality director role 100031 and GM role 100032 in M10.");
            directorId = director.EmpID;
            managerId = manager.EmpID;
            requests = requests.Where(x => x.Totalrmavalues >= 20000 && x.res_id_approver == directorId);
        }
        else
            requests = requests.Where(x => x.Totalrmavalues >= 0 && x.Totalrmavalues < 20000);

        foreach (var request in requests.OrderByDescending(x => x.Id).ToList())
        {
            var customerCode = request.Customer?.Trim();
            if (string.IsNullOrWhiteSpace(customerCode) || !erp.arcusfil_sql.Any(x =>
                x.cus_no.Trim() == customerCode && x.curr_cd.Trim() == "USD")) continue;
            var lines = m10.csexsw_coustumer.AsNoTracking().Where(x => x.RmaId == request.Id).ToList();
            if (lines.Count > short.MaxValue || lines.Any(line => string.IsNullOrWhiteSpace(line.Coustumer)
                || !erp.imitmidx_sql.Any(item => item.item_no == line.Coustumer))) continue;
            if (name == "RMA_TEST_BYPASS_ID" && !m10.humres.Any(x => x.res_id > 0
                && x.res_id != request.res_id_approver && x.usr_id != null && x.usr_id.Trim() != "")) continue;
            if (name == "RMA_TEST_BYPASS_DIRECTOR_ID" && !m10.humres.Any(x => x.res_id > 0
                && x.res_id != directorId && x.res_id != managerId && x.usr_id != null && x.usr_id.Trim() != "")) continue;
            yield return request.Id;
        }
    }

    private static DbContextM10 M10(SaveChangesInterceptor interceptor = null)
    {
        var options = new DbContextOptionsBuilder<DbContextM10>()
            .UseSqlServer(ApprovalTestConfiguration.M10);
        if (interceptor != null) options.AddInterceptors(interceptor);
        return new DbContextM10(options.Options);
    }

    private static DbContext500 Erp(SaveChangesInterceptor interceptor = null)
    {
        var options = new DbContextOptionsBuilder<DbContext500>()
            .UseSqlServer(ApprovalTestConfiguration.ERP);
        if (interceptor != null) options.AddInterceptors(interceptor);
        return new DbContext500(options.Options);
    }

    private static CSEXSW_RmaRepository Repository(DbContextM10 m10, DbContext500 erp) =>
        new(m10, erp, new ConfigurationBuilder().Build());

    private static CSEXSW_RmaRepository BypassRepository(DbContextM10 m10, DbContext500 erp, string username)
    {
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(
            new Dictionary<string, string>
            {
                ["ApprovalBypass:Users:0"] = "  " + username.Trim().ToUpperInvariant() + "  "
            }).Build();
        return new CSEXSW_RmaRepository(m10, erp, configuration);
    }

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
