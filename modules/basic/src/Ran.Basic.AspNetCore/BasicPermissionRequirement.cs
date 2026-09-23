using Microsoft.AspNetCore.Authorization;

namespace Ran.Basic.AspNetCore;

public sealed record BasicPermissionRequirement(string PermissionName) : IAuthorizationRequirement;
