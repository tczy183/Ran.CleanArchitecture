using System.Reflection;

using Application.Common.Exceptions;
using Application.Common.Interfaces;

using CleanArchitecture.Application.Common.Security;

using MediatR;

namespace Application.Common.Behaviours;

public class AuthorizationBehaviour<TRequest, TResponse>(
    IUser user,
    IIdentityService identityService) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var authorizeAttributes =
            request.GetType().GetCustomAttributes<AuthorizeAttribute>().ToList();

        if (!authorizeAttributes.Any())
        {
            return await next();
        }

        // Must be authenticated user
        if (user.Id == null)
        {
            throw new UnauthorizedAccessException();
        }

        // Role-based authorization
        var authorizeAttributesWithRoles =
            authorizeAttributes.Where(a => !string.IsNullOrWhiteSpace(a.Roles)).ToList();

        if (authorizeAttributesWithRoles.Any())
        {
            bool authorized = false;

            foreach (string[] roles in authorizeAttributesWithRoles.Select(a => a.Roles.Split(',')))
            {
                foreach (string role in roles)
                {
                    bool isInRole = await identityService.IsInRoleAsync(user.Id, role.Trim());
                    if (isInRole)
                    {
                        authorized = true;
                        break;
                    }
                }
            }

            // Must be a member of at least one role in roles
            if (!authorized)
            {
                throw new ForbiddenAccessException();
            }
        }

        // Policy-based authorization
        var authorizeAttributesWithPolicies =
            authorizeAttributes.Where(a => !string.IsNullOrWhiteSpace(a.Policy)).ToList();
        if (authorizeAttributesWithPolicies.Any())
        {
            foreach (string policy in authorizeAttributesWithPolicies.Select(a => a.Policy))
            {
                bool authorized = await identityService.AuthorizeAsync(user.Id, policy);

                if (!authorized)
                {
                    throw new ForbiddenAccessException();
                }
            }
        }

        // User is authorized / authorization not required
        return await next();
    }
}