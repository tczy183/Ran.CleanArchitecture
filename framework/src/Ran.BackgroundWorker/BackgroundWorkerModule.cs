using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Ran.Core.Application;
using Ran.Core.BackgroundWorkers;
using Ran.Core.Modularity;
using Ran.Core.Utils.Threading;

namespace Ran.BackgroundWorker;

public class BackgroundWorkerModule : DddModule
{
    public override async Task OnApplicationInitializationAsync(
        ApplicationInitializationContext context
    )
    {
        var options = context
            .ServiceProvider.GetRequiredService<IOptions<BackgroundWorkerOptions>>()
            .Value;
        if (options.IsEnabled)
        {
            var hostApplicationLifetime =
                context.ServiceProvider.GetService<IHostApplicationLifetime>();
            var cancellationToken =
                hostApplicationLifetime?.ApplicationStopping ?? CancellationToken.None;
            await context
                .ServiceProvider.GetRequiredService<IBackgroundWorkerManager>()
                .StartAsync(cancellationToken);
        }
    }

    public override async Task OnApplicationShutdownAsync(ApplicationShutdownContext context)
    {
        var options = context
            .ServiceProvider.GetRequiredService<IOptions<BackgroundWorkerOptions>>()
            .Value;
        if (options.IsEnabled)
        {
            var hostApplicationLifetime =
                context.ServiceProvider.GetService<IHostApplicationLifetime>();
            var cancellationToken =
                hostApplicationLifetime?.ApplicationStopping ?? CancellationToken.None;
            await context
                .ServiceProvider.GetRequiredService<IBackgroundWorkerManager>()
                .StopAsync(cancellationToken);
        }
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        AsyncHelper.RunSync(() => OnApplicationInitializationAsync(context));
    }

    public override void OnApplicationShutdown(ApplicationShutdownContext context)
    {
        AsyncHelper.RunSync(() => OnApplicationShutdownAsync(context));
    }
}
