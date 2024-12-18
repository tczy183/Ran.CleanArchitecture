namespace Ran.Core.Ran.Modularity.Abstractions;

public interface IModule
    : IPreConfigureServices,
        IPostConfigureServices,
        IOnPreApplicationInitialization,
        IOnApplicationInitialization,
        IOnPostApplicationInitialization,
        IOnApplicationShutdown
{
    void ConfigureServices(IApplicationConfigureServiceContext context);
    Task ConfigureServicesAsync(IApplicationConfigureServiceContext context);
}
