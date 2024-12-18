namespace Ran.Core.Ran.Modularity.Abstractions;

public interface IPreConfigureServices
{
    void PreConfigureServices(IApplicationConfigureServiceContext context);
    Task PreConfigureServicesAsync(IApplicationConfigureServiceContext context);
}
