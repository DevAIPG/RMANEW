using Amphenol.RMA.AccesoDatos.Data;
using Amphenol.RMA.Models;
using Microsoft.EntityFrameworkCore;

internal static class ApprovalFixtureFactory
{
    public static int Create(DbContextM10 m10, DbContext500 erp, string scenario, IReadOnlyCollection<int> excluded)
    {
        // Check both destinations here too, before opening connections or inserting fixtures.
        ApprovalTestConnection.Validate(m10.Database.GetConnectionString());
        ApprovalTestConnection.Validate(erp.Database.GetConnectionString());
        var directorStage = scenario == "RMA_TEST_BYPASS_DIRECTOR_ID";
        var directorId = 0;
        var managerId = 0;
        if (directorStage)
        {
            directorId = m10.HRRoles.FirstOrDefault(x => x.RoleID == 100031)?.EmpID ?? 0;
            managerId = m10.HRRoles.FirstOrDefault(x => x.RoleID == 100032)?.EmpID ?? 0;
            if (directorId <= 0 || managerId <= 0 || directorId == managerId
                || !m10.humres.Any(x => x.res_id == directorId)
                || !m10.humres.Any(x => x.res_id == managerId))
                throw new InvalidOperationException("The director-stage fixture requires distinct existing employees in director role 100031 and GM role 100032.");
        }

        // Approved requests are reusable templates, so repeated runs do not exhaust pending data.
        // Copy only scalar fields and lines; never reset or update the source request.
        var templates = m10.CSEXSW_Rma.AsNoTracking()
            .Where(x => x.Customer != null && x.Ship_To != null
                && m10.csexsw_coustumer.Any(line => line.RmaId == x.Id))
            .OrderByDescending(x => x.Status == "Approved").ThenByDescending(x => x.Id).ToList();
        foreach (var source in templates)
        {
            var customerCode = source.Customer.Trim();
            if (!erp.arcusfil_sql.Any(x => x.cus_no.Trim() == customerCode && x.curr_cd.Trim() == "USD")) continue;
            var approver = directorStage
                ? m10.humres.FirstOrDefault(x => x.res_id == directorId)
                : m10.humres.FirstOrDefault(x => x.res_id == source.res_id_approver && x.res_id > 0)
                    ?? m10.humres.FirstOrDefault(x => x.res_id > 0);
            if (approver == null) continue;
            if ((scenario == "RMA_TEST_BYPASS_ID" || directorStage)
                && !m10.humres.Any(x => x.res_id > 0 && x.res_id != approver.res_id
                    && (!directorStage || x.res_id != managerId)
                    && x.usr_id != null && x.usr_id.Trim() != "")) continue;
            var lines = m10.csexsw_coustumer.AsNoTracking().Where(x => x.RmaId == source.Id).OrderBy(x => x.Id).ToList();
            if (lines.Count == 0 || lines.Count > short.MaxValue
                || lines.Any(line => string.IsNullOrWhiteSpace(line.Coustumer)
                    || !erp.imitmidx_sql.Any(x => x.item_no == line.Coustumer))) continue;

            string requestNumber;
            do { requestNumber = "T" + Guid.NewGuid().ToString("N").Substring(0, 7).ToUpperInvariant(); }
            while (m10.CSEXSW_Rma.Any(x => x.Rmarequest == requestNumber));
            var fixture = Build(m10, source, lines, approver.res_id, approver.fullname,
                scenario, requestNumber, directorStage);

            using var transaction = m10.Database.BeginTransaction();
            m10.CSEXSW_Rma.Add(fixture.Request);
            m10.SaveChanges();
            if (excluded.Contains(fixture.Request.Id))
            {
                // An explicit override might reserve a not-yet-existing identity value.
                transaction.Rollback();
                m10.ChangeTracker.Clear();
                continue;
            }
            foreach (var line in fixture.Lines) line.RmaId = fixture.Request.Id;
            m10.csexsw_coustumer.AddRange(fixture.Lines);
            m10.SaveChanges();
            transaction.Commit();
            return fixture.Request.Id;
        }
        throw new InvalidOperationException(
            $"Cannot create a fixture for {scenario}: the test databases need at least one reusable USD request " +
            "with shipping information and existing ERP items, plus the required employee accounts. " +
            "The template may already be approved; no fresh pending or high-value request is required. " +
            "No ERP master data is created or changed by fixture setup.");
    }

    internal static (CSEXSW_Rma Request, List<csexsw_coustumer> Lines) Build(DbContextM10 m10,
        CSEXSW_Rma source, IReadOnlyList<csexsw_coustumer> sourceLines, int approverId, string approverName,
        string scenario, string requestNumber, bool directorStage)
    {
        var request = (CSEXSW_Rma)m10.Entry(source).CurrentValues.ToObject();
        request.Id = 0;
        request.Rmarequest = requestNumber;
        request.Status = "Pending";
        request.Sumbit = "Submitted";
        request.turno = "";
        request.date_approved = null;
        request.Crearcar = false;
        request.Car = "";
        request.Date = DateTime.Now.ToString("MM/dd/yyyy");
        request.Comment = $"Approval integration fixture: {scenario}; template Id={source.Id}";
        request.res_id_approver = approverId;
        request.Approver = approverName;
        request.Totalrmavalues = directorStage ? 25000 : 1000;

        if (sourceLines.Count == 0) throw new ArgumentException("A fixture template needs at least one line.", nameof(sourceLines));
        var templates = sourceLines.ToList();
        // Ensure the invalid-later-line test has a valid earlier line to exercise preflight ordering.
        if (templates.Count == 1) templates.Add(templates[0]);
        var lines = templates.Select(line => (csexsw_coustumer)m10.Entry(line).CurrentValues.ToObject()).ToList();
        var total = (decimal)request.Totalrmavalues;
        var unit = Math.Round(total / lines.Count, 6);
        for (var index = 0; index < lines.Count; index++)
        {
            var line = lines[index];
            line.Id = 0;
            line.RmaId = 0;
            line.CSEXSW_Rma = null;
            line.rma_seq_no = 0;
            line.Car = false;
            line.Qty = 1;
            line.Unit = index == lines.Count - 1 ? total - unit * (lines.Count - 1) : unit;
        }
        return (request, lines);
    }
}
