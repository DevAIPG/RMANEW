using Amphenol.RMA.AccesoDatos.Data;
using Amphenol.RMA.Models;

internal static class ApprovalTestRoleAssignments
{
    public const int QualityManagerId = 990000001;
    public const int PlantManagerId = 990000002;
    public const int BusinessUnitDirectorId = 990000003;
    public const int GeneralManagerId = 990000004;
    public const int CustomerServiceManagerId = 990000005;
    public const int ItBypassId = 990000006;

    public static CSEXSW_RmaRoleAssignment Ensure(DbContextM10 m10, string roleTitle,
        string location, int employeeNumber, bool isActive = true)
    {
        var role = m10.RmaRoles.FirstOrDefault(x => x.Title == roleTitle);
        if (role == null)
        {
            role = new CSEXSW_RmaRole { Id = Guid.NewGuid(), Title = roleTitle };
            m10.RmaRoles.Add(role);
            m10.SaveChanges();
        }

        var assignment = m10.RmaRoleAssignments.FirstOrDefault(x =>
            x.RmaRoleId == role.Id && x.Location == location);
        if (assignment == null)
        {
            assignment = new CSEXSW_RmaRoleAssignment
            {
                Id = Guid.NewGuid(),
                RmaRoleId = role.Id,
                Location = location
            };
            m10.RmaRoleAssignments.Add(assignment);
        }

        assignment.EmployeeNumber = employeeNumber;
        assignment.DisplayName = $"Approval Test {roleTitle}";
        assignment.Email = $"approval-test-{employeeNumber}@example.test";
        assignment.IsActive = isActive;
        m10.SaveChanges();
        return assignment;
    }
}
