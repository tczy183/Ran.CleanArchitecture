namespace Ran.Basic.Entities;

public sealed class BasicUserRole
{
    public Guid UserId { get; set; }

    public Guid RoleId { get; set; }
}

public sealed class BasicRoleMenu
{
    public Guid RoleId { get; set; }

    public Guid MenuId { get; set; }
}

public sealed class BasicRoleDepartment
{
    public Guid RoleId { get; set; }

    public Guid DepartmentId { get; set; }
}
