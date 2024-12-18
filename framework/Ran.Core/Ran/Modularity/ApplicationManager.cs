using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ran.Core.Ran.Modularity.Abstractions;

namespace Ran.Core.Ran.Modularity;

public class ApplicationManager : IApplicationManager
{
    public Type StartupModuleType { get; }

    public IReadOnlyList<IModuleDescriptor> Modules { get; }

    public ApplicationManager(Type startupModuleType, IServiceCollection services)
    {
        if (services == null)
            throw new ArgumentNullException(nameof(services));
        StartupModuleType =
            startupModuleType ?? throw new ArgumentNullException(nameof(startupModuleType));

        services.TryAddSingleton<IApplicationManager>(this);
        services.TryAddSingleton<IModuleLoader>(new ModuleLoader());
        Modules = LoadModules(services);
        ConfigureConfigureServices(services);
    }

    public void ConfigureConfigureServices(IServiceCollection services)
    {
        var context = new ApplicationConfigureServiceContext(services);
        foreach (var moduleDescriptor in Modules)
        {
            try
            {
                moduleDescriptor.Instance.PreConfigureServices(context);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "An error occurred during PreConfigureServices phase of the module "
                        + moduleDescriptor.ModuleType.AssemblyQualifiedName
                        + ". See the inner exception for details.",
                    ex
                );
            }
        }

        foreach (var moduleDescriptor in Modules)
        {
            try
            {
                moduleDescriptor.Instance.ConfigureServices(context);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "An error occurred during ConfigureServices phase of the module "
                        + moduleDescriptor.ModuleType.AssemblyQualifiedName
                        + ". See the inner exception for details.",
                    ex
                );
            }
        }

        foreach (var moduleDescriptor in Modules)
        {
            try
            {
                moduleDescriptor.Instance.PostConfigureServices(context);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "An error occurred during PostConfigureServices phase of the module "
                        + moduleDescriptor.ModuleType.AssemblyQualifiedName
                        + ". See the inner exception for details.",
                    ex
                );
            }
        }
    }

    public void Configure(
        IApplicationBuilder applicationBuilder,
        IEndpointRouteBuilder endpointRouteBuilder
    )
    {
        var context = new ApplicationInitializationContext(
            applicationBuilder,
            endpointRouteBuilder
        );
        foreach (var moduleDescriptor in Modules)
        {
            try
            {
                moduleDescriptor.Instance.OnPreApplicationInitialization(context);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "An error occurred during PreConfigure phase of the module "
                        + moduleDescriptor.ModuleType.AssemblyQualifiedName
                        + ". See the inner exception for details.",
                    ex
                );
            }
        }

        foreach (var moduleDescriptor in Modules)
        {
            try
            {
                moduleDescriptor.Instance.OnApplicationInitialization(context);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "An error occurred during Configure phase of the module "
                        + moduleDescriptor.ModuleType.AssemblyQualifiedName
                        + ". See the inner exception for details.",
                    ex
                );
            }
        }

        foreach (var moduleDescriptor in Modules)
        {
            try
            {
                moduleDescriptor.Instance.OnPostApplicationInitialization(context);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "An error occurred during PostConfigure phase of the module "
                        + moduleDescriptor.ModuleType.AssemblyQualifiedName
                        + ". See the inner exception for details.",
                    ex
                );
            }
        }
    }

    public void Shutdown(
        IApplicationBuilder applicationBuilder,
        IEndpointRouteBuilder endpointRouteBuilder
    )
    {
        var context = new ApplicationInitializationContext(
            applicationBuilder,
            endpointRouteBuilder
        );
        var modules = Modules.Reverse().ToList();
        foreach (var moduleDescriptor in modules)
        {
            try
            {
                moduleDescriptor.Instance.OnApplicationShutdown(context);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "An error occurred during Shutdown phase of the module  "
                        + moduleDescriptor.ModuleType.AssemblyQualifiedName
                        + ". See the inner exception for details.",
                    ex
                );
            }
        }
    }

    private IReadOnlyList<IModuleDescriptor> LoadModules(IServiceCollection services)
    {
        var moduleLoader = (IModuleLoader)
            services
                .FirstOrDefault(p => p.ServiceType == typeof(IModuleLoader))
                ?.ImplementationInstance!;
        return moduleLoader?.LoadModules(services, StartupModuleType);
    }
}
