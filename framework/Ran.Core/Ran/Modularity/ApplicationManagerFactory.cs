using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ran.Core.Ran.Modularity.Abstractions;

namespace Ran.Core.Ran.Modularity;

public static class ApplicationManagerFactory
{
    public static IApplicationManager CreateApplication(
        Type startupModuleType,
        IServiceCollection services
    )
    {
        return new ApplicationManager(startupModuleType, services);
    }
}
