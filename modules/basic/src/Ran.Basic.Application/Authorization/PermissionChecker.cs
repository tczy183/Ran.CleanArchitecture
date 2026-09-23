using Ran.Core.DependencyInjection.ServiceLifetimes;

namespace Ran.Basic.Authorization;

public sealed class PermissionChecker(IBasicAuthorizationStore store)
    : IPermissionChecker,
        IScopedDependency
{
    public async Task<bool> IsGrantedAsync(
        Guid userId,
        string permissionName,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(permissionName);
        var user = await store.FindUserAsync(userId, cancellationToken);
        if (user is not { IsActive: true })
        {
            return false;
        }

        if (!await store.IsPermissionEnabledAsync(permissionName, cancellationToken))
        {
            return false;
        }

        var userGrant = await store.FindUserPermissionGrantAsync(
            userId,
            permissionName,
            cancellationToken
        );
        return userGrant
            ?? await store.IsPermissionGrantedToAnyRoleAsync(
                userId,
                permissionName,
                cancellationToken
            );
    }
}
