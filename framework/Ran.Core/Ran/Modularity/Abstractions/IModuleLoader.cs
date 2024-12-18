using Microsoft.Extensions.DependencyInjection;

namespace Ran.Core.Ran.Modularity.Abstractions;

public interface IModuleLoader
{
    IModuleDescriptor[] LoadModules(IServiceCollection services, Type startupModuleType);
}
