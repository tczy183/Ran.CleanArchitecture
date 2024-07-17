using System.Security.Claims;

using Application.Common.Interfaces;
using Application.Common.Models;

using Volo.Abp.DependencyInjection;

namespace Infrastructure.Identity;

public class IdentityService : IIdentityService, ISingletonDependency
{
    public async Task<string?> GetUserNameAsync(string userId)
    {
        await Task.CompletedTask;
        throw new NotImplementedException();
    }

    public async Task<bool> IsInRoleAsync(string userId, string role)
    {
        await Task.CompletedTask;
        throw new NotImplementedException();
    }

    public async Task<bool> AuthorizeAsync(string userId, string policyName)
    {
        await Task.CompletedTask;
        throw new NotImplementedException();
    }

    public async Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password)
    {
        await Task.CompletedTask;
        throw new NotImplementedException();
    }

    public async Task<Result> DeleteUserAsync(string userId)
    {
        await Task.CompletedTask;
        throw new NotImplementedException();
    }
}

