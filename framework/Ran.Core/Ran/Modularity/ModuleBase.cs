using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ran.Core.Ran.Modularity.Abstractions;

namespace Ran.Core.Ran.Modularity;

public abstract class BaseModule : IModule
{
    public static bool IsModule(Type type)
    {
        return typeof(IModule).IsAssignableFrom(type)
            && !type.IsAbstract
            && type is { IsInterface: false, IsClass: true };
    }

    internal static void CheckModuleType(Type moduleType)
    {
        if (!IsModule(moduleType))
        {
            throw new ArgumentException(
                "Given type is not an module: " + moduleType.AssemblyQualifiedName
            );
        }
    }

    public virtual void ConfigureServices(IApplicationConfigureServiceContext context) { }

    public virtual async Task ConfigureServicesAsync(IApplicationConfigureServiceContext context)
    {
        ConfigureServices(context);
        await Task.CompletedTask;
    }

    public virtual void PreConfigureServices(IApplicationConfigureServiceContext context) { }

    public virtual async Task PreConfigureServicesAsync(IApplicationConfigureServiceContext context)
    {
        PreConfigureServices(context);
        await Task.CompletedTask;
    }

    public virtual void PostConfigureServices(IApplicationConfigureServiceContext context) { }

    public virtual async Task PostConfigureServicesAsync(
        IApplicationConfigureServiceContext context
    )
    {
        PostConfigureServices(context);
        await Task.CompletedTask;
    }

    public virtual void OnPreApplicationInitialization(
        IApplicationInitializationContext context
    ) { }

    public virtual async Task OnPreApplicationInitializationAsync(
        IApplicationInitializationContext context
    )
    {
        OnPreApplicationInitialization(context);
        await Task.CompletedTask;
    }

    public virtual void OnApplicationInitialization(IApplicationInitializationContext context) { }

    public virtual async Task OnApplicationInitializationAsync(
        IApplicationInitializationContext context
    )
    {
        OnApplicationInitialization(context);
        await Task.CompletedTask;
    }

    public virtual void OnPostApplicationInitialization(
        IApplicationInitializationContext context
    ) { }

    public virtual async Task OnPostApplicationInitializationAsync(
        IApplicationInitializationContext context
    )
    {
        OnPostApplicationInitialization(context);
        await Task.CompletedTask;
    }

    public virtual void OnApplicationShutdown(IApplicationInitializationContext context) { }

    public virtual async Task OnApplicationShutdownAsync(IApplicationInitializationContext context)
    {
        OnApplicationShutdown(context);
        await Task.CompletedTask;
    }
}
