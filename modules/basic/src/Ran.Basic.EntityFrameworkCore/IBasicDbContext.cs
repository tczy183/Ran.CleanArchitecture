using Microsoft.EntityFrameworkCore;
using Ran.Basic.Entities;

namespace Ran.Basic.EntityFrameworkCore;

public interface IBasicDbContext
{
    DbSet<BasicUser> Users { get; }

    DbSet<BasicRole> Roles { get; }

    DbSet<BasicMenu> Menus { get; }

    DbSet<BasicDepartment> Departments { get; }

    DbSet<BasicPermission> Permissions { get; }

    DbSet<BasicPermissionGrant> PermissionGrants { get; }

    DbSet<BasicUserRole> UserRoles { get; }

    DbSet<BasicRoleMenu> RoleMenus { get; }

    DbSet<BasicRoleDepartment> RoleDepartments { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
