namespace Ran.Core.Ran.Modularity.Abstractions;

public interface IOnApplicationShutdown
{
    void OnApplicationShutdown(IApplicationInitializationContext context);
    Task OnApplicationShutdownAsync(IApplicationInitializationContext context);
}
