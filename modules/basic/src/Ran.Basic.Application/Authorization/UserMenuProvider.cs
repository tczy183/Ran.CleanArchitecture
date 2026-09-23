using Ran.Basic.Entities;
using Ran.Core.DependencyInjection.ServiceLifetimes;

namespace Ran.Basic.Authorization;

public sealed class UserMenuProvider(
    IBasicAuthorizationStore store,
    IPermissionChecker permissionChecker
) : IUserMenuProvider, IScopedDependency
{
    public async Task<IReadOnlyList<BasicMenu>> GetMenusAsync(
        Guid userId,
        CancellationToken cancellationToken = default
    )
    {
        var user = await store.FindUserAsync(userId, cancellationToken);
        if (user is not { IsActive: true })
        {
            return [];
        }

        var assignedMenus = await store.GetGrantedMenusAsync(userId, cancellationToken);
        var grantedMenus = new List<BasicMenu>(assignedMenus.Count);
        foreach (var menu in assignedMenus)
        {
            if (
                menu.PermissionName is null
                || await permissionChecker.IsGrantedAsync(
                    userId,
                    menu.PermissionName,
                    cancellationToken
                )
            )
            {
                grantedMenus.Add(menu);
            }
        }

        return grantedMenus;
    }
}
