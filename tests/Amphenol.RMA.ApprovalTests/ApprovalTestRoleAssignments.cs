using Amphenol.RMA.AccesoDatos.Data;
using Amphenol.RMA.Models;
using Microsoft.EntityFrameworkCore;

internal static class ApprovalTestRoleAssignments
{
    public static CSEXSW_RmaRoleAssignment RequireExisting(DbContextM10 m10, string roleTitle,
        string location, int? expectedEmployeeNumber = null)
    {
        var assignment = FindExisting(m10, roleTitle, location);

        if (assignment == null)
            throw new InvalidOperationException(
                $"The approval test database is missing an active {roleTitle} assignment for {location}. " +
                "Apply the approved CSEXSW_RmaRole and CSEXSW_RmaRoleAssignment seed script; tests do not create or modify role assignments.");

        if (assignment.EmployeeNumber is not > 0)
            throw new InvalidOperationException(
                $"The active {roleTitle} assignment for {location} has no valid employee number. " +
                "Apply the approved role assignment seed script; tests do not create or modify role assignments.");

        if (expectedEmployeeNumber.HasValue && assignment.EmployeeNumber != expectedEmployeeNumber)
            throw new InvalidOperationException(
                $"The active {roleTitle} assignment for {location} has employee number {assignment.EmployeeNumber}, " +
                $"but the request expects {expectedEmployeeNumber}. Tests do not replace existing role assignments.");

        return assignment;
    }

    public static CSEXSW_RmaRoleAssignment? FindExisting(DbContextM10 m10, string roleTitle, string location) =>
        m10.RmaRoleAssignments.AsNoTracking()
            .Include(x => x.RmaRole)
            .Where(x => x.IsActive && x.Location == location && x.RmaRole.Title == roleTitle)
            .OrderBy(x => x.Id).FirstOrDefault();
}
