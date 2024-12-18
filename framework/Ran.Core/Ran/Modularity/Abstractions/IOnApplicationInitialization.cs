namespace Ran.Core.Ran.Modularity.Abstractions;

public interface IOnApplicationInitialization
{
    void OnApplicationInitialization(IApplicationInitializationContext context);
    Task OnApplicationInitializationAsync(IApplicationInitializationContext context);
}
