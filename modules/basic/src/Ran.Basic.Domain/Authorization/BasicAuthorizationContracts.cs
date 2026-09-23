using Ran.Basic.Entities;

namespace Ran.Basic.Authorization;

public sealed record RoleDataGrant(
    Guid RoleId,
    DataScope DataScope,
    IReadOnlyCollection<Guid> DepartmentIds
);

public interface IBasicAuthorizationStore
{
    Task<BasicUser?> FindUserAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<bool> IsPermissionEnabledAsync(
        string permissionName,
        CancellationToken cancellationToken = default
    );

    Task<bool?> FindUserPermissionGrantAsync(
        Guid userId,
        string permissionName,
        CancellationToken cancellationToken = default
    );

    Task<bool> IsPermissionGrantedToAnyRoleAsync(
        Guid userId,
        string permissionName,
        CancellationToken cancellationToken = default
    );

    Task<IReadOnlyList<BasicMenu>> GetGrantedMenusAsync(
        Guid userId,
        CancellationToken cancellationToken = default
    );

    Task<IReadOnlyList<RoleDataGrant>> GetRoleDataGrantsAsync(
        Guid userId,
        CancellationToken cancellationToken = default
    );

    Task<IReadOnlySet<Guid>> GetDepartmentAndDescendantIdsAsync(
        Guid departmentId,
        CancellationToken cancellationToken = default
    );

    Task ReplaceUserRolesAsync(
        Guid userId,
        IReadOnlyCollection<Guid> roleIds,
        CancellationToken cancellationToken = default
    );

    Task ReplaceRoleMenusAsync(
        Guid roleId,
        IReadOnlyCollection<Guid> menuIds,
        CancellationToken cancellationToken = default
    );

    Task SetRoleDataScopeAsync(
        Guid roleId,
        DataScope dataScope,
        IReadOnlyCollection<Guid> departmentIds,
        CancellationToken cancellationToken = default
    );

    Task SetPermissionGrantAsync(
        PermissionGrantProvider provider,
        Guid providerId,
        string permissionName,
        bool isGranted,
        CancellationToken cancellationToken = default
    );
}

public interface IPermissionChecker
{
    Task<bool> IsGrantedAsync(
        Guid userId,
        string permissionName,
        CancellationToken cancellationToken = default
    );
}

public interface IUserMenuProvider
{
    Task<IReadOnlyList<BasicMenu>> GetMenusAsync(
        Guid userId,
        CancellationToken cancellationToken = default
    );
}

public interface IDataPermissionResolver
{
    Task<DataPermission> ResolveAsync(Guid userId, CancellationToken cancellationToken = default);
}

public interface IBasicAccessManager
{
    Task SetUserRolesAsync(
        Guid userId,
        IReadOnlyCollection<Guid> roleIds,
        CancellationToken cancellationToken = default
    );

    Task SetRoleMenusAsync(
        Guid roleId,
        IReadOnlyCollection<Guid> menuIds,
        CancellationToken cancellationToken = default
    );

    Task SetRoleDataScopeAsync(
        Guid roleId,
        DataScope dataScope,
        IReadOnlyCollection<Guid>? departmentIds = null,
        CancellationToken cancellationToken = default
    );

    Task SetPermissionGrantAsync(
        PermissionGrantProvider provider,
        Guid providerId,
        string permissionName,
        bool isGranted,
        CancellationToken cancellationToken = default
    );
}
