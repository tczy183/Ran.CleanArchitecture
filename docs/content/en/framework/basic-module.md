# Basic Authorization Module

`modules/basic` is a single-tenant authorization template containing users, roles, menus, permissions, departments, and data scopes. Permission definitions, grants, and checks are separate: user grants override role grants, while undefined or disabled permissions are denied by default.

## Module layout

- `Ran.Basic.Domain`: domain entities, permission names, authorization contracts, and data-scope contracts.
- `Ran.Basic.Application`: permission checking, user-menu calculation, data-permission resolution, and grant management.
- `Ran.Basic.EntityFrameworkCore`: EF Core mapping, permission seeds, and persistence.
- `Ran.Basic.AspNetCore`: dynamic ASP.NET Core authorization policies backed by permission checks.

The module does not issue cookies or JWTs. The host can choose its authentication mechanism, but the default user identifier claim is `ClaimTypes.NameIdentifier` with a `Guid` value.

## Connect a DbContext

Implement `IBasicDbContext` on the host DbContext and apply the model configuration:

```csharp
public sealed class AppDbContext(DbContextOptions<AppDbContext> options)
    : DddDbContext<AppDbContext>(options), IBasicDbContext
{
    public DbSet<BasicUser> Users => Set<BasicUser>();
    public DbSet<BasicRole> Roles => Set<BasicRole>();
    public DbSet<BasicMenu> Menus => Set<BasicMenu>();
    public DbSet<BasicDepartment> Departments => Set<BasicDepartment>();
    public DbSet<BasicPermission> Permissions => Set<BasicPermission>();
    public DbSet<BasicPermissionGrant> PermissionGrants => Set<BasicPermissionGrant>();
    public DbSet<BasicUserRole> UserRoles => Set<BasicUserRole>();
    public DbSet<BasicRoleMenu> RoleMenus => Set<BasicRoleMenu>();
    public DbSet<BasicRoleDepartment> RoleDepartments => Set<BasicRoleDepartment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ConfigureBasicModule();
    }
}
```

Depend on both adapters in the host module and replace the authorization store:

```csharp
[DependsOn(typeof(BasicAspNetCoreModule), typeof(BasicEntityFrameworkCoreModule))]
public sealed class WebModule : DddModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddBasicEntityFrameworkCore<AppDbContext>();
    }
}
```

Create and apply an EF Core migration. The built-in `Basic.*` permissions are then inserted into the permission table.

## Functional permissions and menus

Controllers and endpoints can use a dynamic policy directly:

```csharp
[Authorize(Policy = "Permission:Basic.Users")]
public sealed class UsersController : ControllerBase;
```

Use the helper to avoid handwritten policy names:

```csharp
var policy = BasicPermissionPolicy.For(BasicPermissionNames.Users.ManageRoles);
```

Use `IBasicAccessManager` to assign user roles, role menus, user/role permission grants, and role data scopes. Use `IUserMenuProvider` to obtain menus that are both assigned and permitted for a user.

## Data permissions

Roles support these data scopes:

- `None`: no data access.
- `Self`: records owned by the current user.
- `Department`: the current user's department.
- `DepartmentAndChildren`: the current department and all descendants.
- `CustomDepartments`: departments assigned to the role.
- `All`: unrestricted data access and the highest precedence.

Scopes from multiple roles are combined as a union. Implement `IDataPermissionEntity` on a business entity and explicitly apply the resolved permission before executing a query:

```csharp
var permission = await dataPermissionResolver.ResolveAsync(userId, cancellationToken);
var query = dbContext.Orders.WhereDataPermission(permission);
```

Treat the filter as a security boundary: apply it before paging, exports, and aggregation. Hiding a menu in the client is not authorization.
