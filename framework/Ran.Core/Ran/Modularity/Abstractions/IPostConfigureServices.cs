namespace Ran.Core.Ran.Modularity.Abstractions;

public interface IPostConfigureServices
{
    void PostConfigureServices(IApplicationConfigureServiceContext context);
    Task PostConfigureServicesAsync(IApplicationConfigureServiceContext context);
}
