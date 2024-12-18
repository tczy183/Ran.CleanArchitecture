using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ran.Core.Ran.Modularity.Abstractions;

namespace Ran.Core.Ran.Modularity;

public static class ServiceCollectionExtensions
{
    public static void ConfigureServiceCollection<T>(this IServiceCollection service)
        where T : IModule
    {
        ConfigureServiceCollection(service, typeof(T));
    }

    public static void ConfigureServiceCollection(
        this IServiceCollection service,
        Type startupModuleType
    )
    {
        ApplicationManagerFactory.CreateApplication(startupModuleType, service);
    }
}
