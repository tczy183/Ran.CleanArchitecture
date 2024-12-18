using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ran.Core.Ran.Modularity.Abstractions;

namespace Ran.Core.Ran.Modularity;

public class ApplicationConfigureServiceContext(IServiceCollection serviceCollection)
    : IApplicationConfigureServiceContext
{
    public IServiceCollection Services { get; set; } = serviceCollection;
}
