namespace Ran.Core.Ran.Modularity.Abstractions;

public interface IOnPostApplicationInitialization
{
    void OnPostApplicationInitialization(IApplicationInitializationContext context);
    Task OnPostApplicationInitializationAsync(IApplicationInitializationContext context);
}
