using Microsoft.EntityFrameworkCore;
using Ran.Basic.Authorization;
using Ran.Basic.Entities;

namespace Ran.Basic.EntityFrameworkCore;

public sealed class EfCoreBasicAuthorizationStore<TDbContext>(TDbContext dbContext)
    : IBasicAuthorizationStore
    where TDbContext : DbContext, IBasicDbContext
{
    public Task<BasicUser?> FindUserAsync(
        Guid userId,
        CancellationToken cancellationToken = default
    )
    {
        return dbContext
            .Users.AsNoTracking()
            .SingleOrDefaultAsync(user => user.Id == userId, cancellationToken);
    }

    public Task<bool> IsPermissionEnabledAsync(
        string permissionName,
        CancellationToken cancellationToken = default
    )
    {
        return dbContext.Permissions.AnyAsync(
            permission => permission.Name == permissionName && permission.IsEnabled,
            cancellationToken
        );
    }

    public async Task<bool?> FindUserPermissionGrantAsync(
        Guid userId,
        string permissionName,
        CancellationToken cancellationToken = default
    )
    {
        var providerKey = userId.ToString("N");
        return await dbContext
            .PermissionGrants.Where(grant =>
                grant.PermissionName == permissionName
                && grant.Provider == PermissionGrantProvider.User
                && grant.ProviderKey == providerKey
            )
            .Select(grant => (bool?)grant.IsGranted)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> IsPermissionGrantedToAnyRoleAsync(
        Guid userId,
        string permissionName,
        CancellationToken cancellationToken = default
    )
    {
        var roleIds = await GetActiveRoleIdsAsync(userId, cancellationToken);
        var roleKeys = roleIds.Select(roleId => roleId.ToString("N")).ToArray();
        return await dbContext.PermissionGrants.AnyAsync(
            grant =>
                grant.PermissionName == permissionName
                && grant.Provider == PermissionGrantProvider.Role
                && roleKeys.Contains(grant.ProviderKey)
                && grant.IsGranted,
            cancellationToken
        );
    }

    public async Task<IReadOnlyList<BasicMenu>> GetGrantedMenusAsync(
        Guid userId,
        CancellationToken cancellationToken = default
    )
    {
        var roleIds = await GetActiveRoleIdsAsync(userId, cancellationToken);
        return await dbContext
            .Menus.AsNoTracking()
            .Where(menu =>
                menu.IsEnabled
                && menu.IsVisible
                && dbContext.RoleMenus.Any(roleMenu =>
                    roleIds.Contains(roleMenu.RoleId) && roleMenu.MenuId == menu.Id
                )
            )
            .OrderBy(menu => menu.Sort)
            .ThenBy(menu => menu.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<RoleDataGrant>> GetRoleDataGrantsAsync(
        Guid userId,
        CancellationToken cancellationToken = default
    )
    {
        var roles = await (
            from userRole in dbContext.UserRoles.AsNoTracking()
            join role in dbContext.Roles.AsNoTracking() on userRole.RoleId equals role.Id
            where userRole.UserId == userId && role.IsActive
            select new { role.Id, role.DataScope }
        ).ToListAsync(cancellationToken);
        var roleIds = roles.Select(role => role.Id).ToArray();
        var customDepartments = await dbContext
            .RoleDepartments.AsNoTracking()
            .Where(item => roleIds.Contains(item.RoleId))
            .ToListAsync(cancellationToken);
        return roles
            .Select(role => new RoleDataGrant(
                role.Id,
                role.DataScope,
                customDepartments
                    .Where(item => item.RoleId == role.Id)
                    .Select(item => item.DepartmentId)
                    .ToArray()
            ))
            .ToList();
    }

    public async Task<IReadOnlySet<Guid>> GetDepartmentAndDescendantIdsAsync(
        Guid departmentId,
        CancellationToken cancellationToken = default
    )
    {
        var departments = await dbContext
            .Departments.AsNoTracking()
            .Where(department => department.IsActive)
            .Select(department => new { department.Id, department.ParentId })
            .ToListAsync(cancellationToken);
        if (departments.All(department => department.Id != departmentId))
        {
            return new HashSet<Guid>();
        }

        var result = new HashSet<Guid> { departmentId };
        var pending = new Queue<Guid>();
        pending.Enqueue(departmentId);
        while (pending.TryDequeue(out var parentId))
        {
            foreach (
                var childId in departments
                    .Where(department => department.ParentId == parentId)
                    .Select(department => department.Id)
            )
            {
                if (result.Add(childId))
                {
                    pending.Enqueue(childId);
                }
            }
        }

        return result;
    }

    public async Task ReplaceUserRolesAsync(
        Guid userId,
        IReadOnlyCollection<Guid> roleIds,
        CancellationToken cancellationToken = default
    )
    {
        await EnsureUserExistsAsync(userId, cancellationToken);
        await EnsureAllRolesExistAsync(roleIds, cancellationToken);
        var currentRoles = await dbContext
            .UserRoles.Where(userRole => userRole.UserId == userId)
            .ToListAsync(cancellationToken);
        dbContext.UserRoles.RemoveRange(currentRoles);
        await dbContext.UserRoles.AddRangeAsync(
            roleIds.Select(roleId => new BasicUserRole { UserId = userId, RoleId = roleId }),
            cancellationToken
        );
        _ = await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task ReplaceRoleMenusAsync(
        Guid roleId,
        IReadOnlyCollection<Guid> menuIds,
        CancellationToken cancellationToken = default
    )
    {
        await EnsureRoleExistsAsync(roleId, cancellationToken);
        await EnsureAllMenusExistAsync(menuIds, cancellationToken);
        var currentMenus = await dbContext
            .RoleMenus.Where(roleMenu => roleMenu.RoleId == roleId)
            .ToListAsync(cancellationToken);
        dbContext.RoleMenus.RemoveRange(currentMenus);
        await dbContext.RoleMenus.AddRangeAsync(
            menuIds.Select(menuId => new BasicRoleMenu { RoleId = roleId, MenuId = menuId }),
            cancellationToken
        );
        _ = await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task SetRoleDataScopeAsync(
        Guid roleId,
        DataScope dataScope,
        IReadOnlyCollection<Guid> departmentIds,
        CancellationToken cancellationToken = default
    )
    {
        var role =
            await dbContext.Roles.SingleOrDefaultAsync(item => item.Id == roleId, cancellationToken)
            ?? throw new KeyNotFoundException($"Role '{roleId}' was not found.");
        await EnsureAllDepartmentsExistAsync(departmentIds, cancellationToken);
        role.SetDataScope(dataScope);
        var currentDepartments = await dbContext
            .RoleDepartments.Where(item => item.RoleId == roleId)
            .ToListAsync(cancellationToken);
        dbContext.RoleDepartments.RemoveRange(currentDepartments);
        await dbContext.RoleDepartments.AddRangeAsync(
            departmentIds.Select(departmentId => new BasicRoleDepartment
            {
                RoleId = roleId,
                DepartmentId = departmentId,
            }),
            cancellationToken
        );
        _ = await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task SetPermissionGrantAsync(
        PermissionGrantProvider provider,
        Guid providerId,
        string permissionName,
        bool isGranted,
        CancellationToken cancellationToken = default
    )
    {
        if (
            !await dbContext.Permissions.AnyAsync(
                item => item.Name == permissionName,
                cancellationToken
            )
        )
        {
            throw new KeyNotFoundException($"Permission '{permissionName}' was not found.");
        }

        if (provider == PermissionGrantProvider.User)
        {
            await EnsureUserExistsAsync(providerId, cancellationToken);
        }
        else if (provider == PermissionGrantProvider.Role)
        {
            await EnsureRoleExistsAsync(providerId, cancellationToken);
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(provider));
        }

        var providerKey = providerId.ToString("N");
        var grant = await dbContext.PermissionGrants.SingleOrDefaultAsync(
            item =>
                item.PermissionName == permissionName
                && item.Provider == provider
                && item.ProviderKey == providerKey,
            cancellationToken
        );
        if (grant is null)
        {
            await dbContext.PermissionGrants.AddAsync(
                new BasicPermissionGrant(
                    Guid.NewGuid(),
                    permissionName,
                    provider,
                    providerId,
                    isGranted
                ),
                cancellationToken
            );
        }
        else
        {
            grant.SetGranted(isGranted);
        }

        _ = await dbContext.SaveChangesAsync(cancellationToken);
    }

    private Task<Guid[]> GetActiveRoleIdsAsync(Guid userId, CancellationToken cancellationToken)
    {
        return (
            from userRole in dbContext.UserRoles.AsNoTracking()
            join role in dbContext.Roles.AsNoTracking() on userRole.RoleId equals role.Id
            where userRole.UserId == userId && role.IsActive
            select role.Id
        ).ToArrayAsync(cancellationToken);
    }

    private async Task EnsureUserExistsAsync(Guid userId, CancellationToken cancellationToken)
    {
        if (!await dbContext.Users.AnyAsync(user => user.Id == userId, cancellationToken))
        {
            throw new KeyNotFoundException($"User '{userId}' was not found.");
        }
    }

    private async Task EnsureRoleExistsAsync(Guid roleId, CancellationToken cancellationToken)
    {
        if (!await dbContext.Roles.AnyAsync(role => role.Id == roleId, cancellationToken))
        {
            throw new KeyNotFoundException($"Role '{roleId}' was not found.");
        }
    }

    private async Task EnsureAllRolesExistAsync(
        IReadOnlyCollection<Guid> roleIds,
        CancellationToken cancellationToken
    )
    {
        var count = await dbContext.Roles.CountAsync(
            role => roleIds.Contains(role.Id),
            cancellationToken
        );
        if (count != roleIds.Count)
        {
            throw new KeyNotFoundException("One or more roles were not found.");
        }
    }

    private async Task EnsureAllMenusExistAsync(
        IReadOnlyCollection<Guid> menuIds,
        CancellationToken cancellationToken
    )
    {
        var count = await dbContext.Menus.CountAsync(
            menu => menuIds.Contains(menu.Id),
            cancellationToken
        );
        if (count != menuIds.Count)
        {
            throw new KeyNotFoundException("One or more menus were not found.");
        }
    }

    private async Task EnsureAllDepartmentsExistAsync(
        IReadOnlyCollection<Guid> departmentIds,
        CancellationToken cancellationToken
    )
    {
        var count = await dbContext.Departments.CountAsync(
            department => departmentIds.Contains(department.Id),
            cancellationToken
        );
        if (count != departmentIds.Count)
        {
            throw new KeyNotFoundException("One or more departments were not found.");
        }
    }
}
