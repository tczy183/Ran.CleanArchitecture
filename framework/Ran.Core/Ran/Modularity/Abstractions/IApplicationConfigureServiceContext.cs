using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ran.Core.Ran.Modularity.Abstractions;

public interface IApplicationConfigureServiceContext
{
    IServiceCollection Services { get; set; }
}
