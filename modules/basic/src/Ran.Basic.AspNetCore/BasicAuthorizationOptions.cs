using System.Security.Claims;

namespace Ran.Basic.AspNetCore;

public sealed class BasicAuthorizationOptions
{
    public string UserIdClaimType { get; set; } = ClaimTypes.NameIdentifier;

    public string PolicyPrefix { get; set; } = "Permission:";
}
