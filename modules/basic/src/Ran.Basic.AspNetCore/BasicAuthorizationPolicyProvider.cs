using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Ran.Basic.AspNetCore;

public sealed class BasicAuthorizationPolicyProvider(
    IOptions<AuthorizationOptions> authorizationOptions,
    IOptions<BasicAuthorizationOptions> basicOptions
) : DefaultAuthorizationPolicyProvider(authorizationOptions)
{
    private readonly BasicAuthorizationOptions _basicOptions = basicOptions.Value;

    public override Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if (
            !policyName.StartsWith(_basicOptions.PolicyPrefix, StringComparison.Ordinal)
            || policyName.Length == _basicOptions.PolicyPrefix.Length
        )
        {
            return base.GetPolicyAsync(policyName);
        }

        var permissionName = policyName[_basicOptions.PolicyPrefix.Length..];
        var policy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .AddRequirements(new BasicPermissionRequirement(permissionName))
            .Build();
        return Task.FromResult<AuthorizationPolicy?>(policy);
    }
}
