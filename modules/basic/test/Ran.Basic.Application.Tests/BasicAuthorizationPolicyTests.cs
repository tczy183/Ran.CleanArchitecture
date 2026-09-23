using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Ran.Basic.AspNetCore;
using Ran.Basic.Authorization;
using Xunit;

namespace Ran.Basic.Application.Tests;

public sealed class BasicAuthorizationPolicyTests
{
    [Fact]
    public async Task Permission_policy_should_be_created_dynamically()
    {
        var provider = new BasicAuthorizationPolicyProvider(
            Options.Create(new AuthorizationOptions()),
            Options.Create(new BasicAuthorizationOptions())
        );

        var policy = await provider.GetPolicyAsync(
            BasicPermissionPolicy.For(BasicPermissionNames.Users.Default)
        );

        Assert.NotNull(policy);
        var requirement = Assert.Single(policy.Requirements.OfType<BasicPermissionRequirement>());
        Assert.Equal(BasicPermissionNames.Users.Default, requirement.PermissionName);
    }
}
