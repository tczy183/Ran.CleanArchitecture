using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Ran.BackgroundWorker;
using Ran.Core.Application;
using Ran.Core.Modularity;

namespace Ran.BackgroundJob;

[DependsOn(typeof(BackgroundWorkerModule))]
public class BackgroundJobModule : DddModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.TryAddSingleton(TimeProvider.System);
        context.Services.TryAddSingleton<IBackgroundJobStore, InMemoryBackgroundJobStore>();
        context.Services.TryAddTransient<IBackgroundJobManager, DefaultBackgroundJobManager>();
        context.Services.TryAddTransient<IBackgroundJobExecutor, BackgroundJobExecutor>();
        context.Services.TryAddTransient<BackgroundJobWorker>();
    }

    public override async Task OnApplicationInitializationAsync(
        ApplicationInitializationContext context
    )
    {
        var options = context.ServiceProvider.GetRequiredService<IOptions<BackgroundJobOptions>>();
        if (options.Value.IsJobExecutionEnabled)
        {
            await context.AddBackgroundWorkerAsync<BackgroundJobWorker>();
        }
    }
}
