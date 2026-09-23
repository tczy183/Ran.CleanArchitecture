using Ran.Core.DependencyInjection.ServiceLifetimes;

namespace Ran.Basic.Authorization;

public sealed class BasicAccessManager(IBasicAuthorizationStore store)
    : IBasicAccessManager,
        IScopedDependency
{
    public Task SetUserRolesAsync(
        Guid userId,
        IReadOnlyCollection<Guid> roleIds,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(roleIds);
        return store.ReplaceUserRolesAsync(userId, roleIds.Distinct().ToArray(), cancellationToken);
    }

    public Task SetRoleMenusAsync(
        Guid roleId,
        IReadOnlyCollection<Guid> menuIds,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(menuIds);
        return store.ReplaceRoleMenusAsync(roleId, menuIds.Distinct().ToArray(), cancellationToken);
    }

    public Task SetRoleDataScopeAsync(
        Guid roleId,
        DataScope dataScope,
        IReadOnlyCollection<Guid>? departmentIds = null,
        CancellationToken cancellationToken = default
    )
    {
        var distinctDepartmentIds = departmentIds?.Distinct().ToArray() ?? [];
        if (dataScope == DataScope.CustomDepartments && distinctDepartmentIds.Length == 0)
        {
            throw new ArgumentException(
                "Custom department data scope requires at least one department.",
                nameof(departmentIds)
            );
        }

        if (dataScope != DataScope.CustomDepartments && distinctDepartmentIds.Length > 0)
        {
            throw new ArgumentException(
                "Department ids are only valid for the custom department data scope.",
                nameof(departmentIds)
            );
        }

        return store.SetRoleDataScopeAsync(
            roleId,
            dataScope,
            distinctDepartmentIds,
            cancellationToken
        );
    }

    public Task SetPermissionGrantAsync(
        PermissionGrantProvider provider,
        Guid providerId,
        string permissionName,
        bool isGranted,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(permissionName);
        return store.SetPermissionGrantAsync(
            provider,
            providerId,
            permissionName,
            isGranted,
            cancellationToken
        );
    }
}
