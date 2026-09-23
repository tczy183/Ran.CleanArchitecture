using Ran.Core.DependencyInjection.ServiceLifetimes;

namespace Ran.Basic.Authorization;

public sealed class DataPermissionResolver(IBasicAuthorizationStore store)
    : IDataPermissionResolver,
        IScopedDependency
{
    public async Task<DataPermission> ResolveAsync(
        Guid userId,
        CancellationToken cancellationToken = default
    )
    {
        var user = await store.FindUserAsync(userId, cancellationToken);
        if (user is not { IsActive: true })
        {
            return DataPermission.Denied(userId);
        }

        var grants = await store.GetRoleDataGrantsAsync(userId, cancellationToken);
        if (grants.Any(grant => grant.DataScope == DataScope.All))
        {
            return new DataPermission(
                userId,
                AllowsAll: true,
                AllowsSelf: true,
                new HashSet<Guid>()
            );
        }

        var allowsSelf = grants.Any(grant => grant.DataScope == DataScope.Self);
        var departmentIds = new HashSet<Guid>();
        foreach (var grant in grants)
        {
            switch (grant.DataScope)
            {
                case DataScope.Department when user.DepartmentId.HasValue:
                    _ = departmentIds.Add(user.DepartmentId.Value);
                    break;
                case DataScope.DepartmentAndChildren when user.DepartmentId.HasValue:
                    departmentIds.UnionWith(
                        await store.GetDepartmentAndDescendantIdsAsync(
                            user.DepartmentId.Value,
                            cancellationToken
                        )
                    );
                    break;
                case DataScope.CustomDepartments:
                    departmentIds.UnionWith(grant.DepartmentIds);
                    break;
            }
        }

        return new DataPermission(userId, AllowsAll: false, allowsSelf, departmentIds);
    }
}
