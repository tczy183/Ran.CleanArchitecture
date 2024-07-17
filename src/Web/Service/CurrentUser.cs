using System.Security.Claims;

using Application.Common.Interfaces;

using Volo.Abp.DependencyInjection;

namespace Web.Service;

public class CurrentUser(IHttpContextAccessor httpContextAccessor) : IUser, IScopedDependency
{
    public string? Id => httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
}