using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Ran.Basic.Authorization;

namespace Ran.Basic.AspNetCore;

public sealed class BasicPermissionAuthorizationHandler(
    IPermissionChecker permissionChecker,
    IOptions<BasicAuthorizationOptions> options
) : AuthorizationHandler<BasicPermissionRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        BasicPermissionRequirement requirement
    )
    {
        var userIdValue = context.User.FindFirstValue(options.Value.UserIdClaimType);
        if (
            Guid.TryParse(userIdValue, out var userId)
            && await permissionChecker.IsGrantedAsync(userId, requirement.PermissionName)
        )
        {
            context.Succeed(requirement);
        }
    }
}
