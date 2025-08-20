using Ran.Core.Modularity;
using Ran.Mediator.Extensions;

namespace Ran.Mediator;

public sealed class MediatorModule : DddModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddDispatchR();
    }
}
