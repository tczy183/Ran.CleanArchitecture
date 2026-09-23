using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ran.Core.AspNetCore;
using Ran.Core.Modularity;

namespace Ran.Basic.AspNetCore;

[DependsOn(typeof(BasicApplicationModule), typeof(AspNetCoreModule))]
public sealed class BasicAspNetCoreModule : DddModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAuthorization();
        context.Services.Replace(
            ServiceDescriptor.Singleton<
                IAuthorizationPolicyProvider,
                BasicAuthorizationPolicyProvider
            >()
        );
        context.Services.TryAddEnumerable(
            ServiceDescriptor.Scoped<IAuthorizationHandler, BasicPermissionAuthorizationHandler>()
        );
    }
}
