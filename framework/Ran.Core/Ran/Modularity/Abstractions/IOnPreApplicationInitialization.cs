namespace Ran.Core.Ran.Modularity.Abstractions;

public interface IOnPreApplicationInitialization
{
    void OnPreApplicationInitialization(IApplicationInitializationContext context);
    Task OnPreApplicationInitializationAsync(IApplicationInitializationContext context);
}
