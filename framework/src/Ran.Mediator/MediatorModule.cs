using Microsoft.Extensions.Configuration;
using Ran.Core.Extensions.DependencyInjection;
using Ran.Core.Modularity;
using Ran.Mediator.Extensions;

namespace Ran.Mediator;

public sealed class MediatorModule : DddModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // var configuration = context.Services.GetConfiguration();
        context.Services.AddMediator();
    }
}
