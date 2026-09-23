# 基础权限模块

`modules/basic` 是不包含多租户的基础权限模板，提供用户、角色、菜单、权限、部门和数据范围模型。设计上将权限定义、权限授权和权限检查分开：角色授权可被用户级授权覆盖，未定义或已禁用的权限默认拒绝。

## 模块结构

- `Ran.Basic.Domain`：领域实体、权限名称、授权与数据权限契约。
- `Ran.Basic.Application`：权限检查、用户菜单计算、数据权限解析和授权管理。
- `Ran.Basic.EntityFrameworkCore`：EF Core 映射、权限种子和持久化实现。
- `Ran.Basic.AspNetCore`：将权限名称映射为 ASP.NET Core 动态授权策略。

模块不负责签发 Cookie 或 JWT。宿主可以选择自己的认证方式，但用户标识 Claim 默认必须是 `ClaimTypes.NameIdentifier` 且值为 `Guid`。

## 接入 DbContext

宿主 DbContext 实现 `IBasicDbContext`，并应用模块模型：

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

宿主模块依赖两个适配模块，并替换默认授权存储：

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

创建并应用 EF Core migration 后，内置 `Basic.*` 权限会写入权限表。

## 功能权限与菜单

控制器或端点可以直接使用动态策略：

```csharp
[Authorize(Policy = "Permission:Basic.Users")]
public sealed class UsersController : ControllerBase;
```

也可以避免手写策略名：

```csharp
var policy = BasicPermissionPolicy.For(BasicPermissionNames.Users.ManageRoles);
```

使用 `IBasicAccessManager` 分配用户角色、角色菜单、角色/用户权限以及角色数据范围。使用 `IUserMenuProvider` 获取当前用户可见且权限检查通过的菜单。

## 数据权限

角色支持以下数据范围：

- `None`：不允许访问数据。
- `Self`：仅记录所有者为当前用户的数据。
- `Department`：当前用户所属部门。
- `DepartmentAndChildren`：当前部门及所有下级部门。
- `CustomDepartments`：角色配置的部门集合。
- `All`：全部数据，优先级最高。

同一用户的多个角色取并集。业务实体实现 `IDataPermissionEntity` 后，在查询中显式应用解析结果：

```csharp
var permission = await dataPermissionResolver.ResolveAsync(userId, cancellationToken);
var query = dbContext.Orders.WhereDataPermission(permission);
```

数据权限是安全边界，应在返回分页、导出或聚合数据之前应用。不要只在前端隐藏菜单，也不要将 `IgnoreQueryFilters` 作为绕过方式。
