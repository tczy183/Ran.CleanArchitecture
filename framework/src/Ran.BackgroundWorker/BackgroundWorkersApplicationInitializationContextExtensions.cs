using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ran.Core.Application;
using Ran.Core.BackgroundWorkers;
using Ran.Core.Exceptions;
using Ran.Core.Utils.System;

namespace Ran.BackgroundWorker;

public static class BackgroundWorkersApplicationInitializationContextExtensions
{
    public async static Task<ApplicationInitializationContext> AddBackgroundWorkerAsync<TWorker>(
        [NotNull] this ApplicationInitializationContext context, CancellationToken cancellationToken = default)
        where TWorker : IBackgroundWorker
    {
        CheckHelper.NotNull(context, nameof(context));

        await context.AddBackgroundWorkerAsync(typeof(TWorker), cancellationToken: cancellationToken);

        return context;
    }

    public async static Task<ApplicationInitializationContext> AddBackgroundWorkerAsync(
        [NotNull] this ApplicationInitializationContext context, [NotNull] Type workerType,
        CancellationToken cancellationToken = default)
    {
        CheckHelper.NotNull(context, nameof(context));
        CheckHelper.NotNull(workerType, nameof(workerType));

        if (!workerType.IsAssignableTo<IBackgroundWorker>())
        {
            throw new UserFriendlyException(
                $"Given type ({workerType.AssemblyQualifiedName}) must implement the {typeof(IBackgroundWorker).AssemblyQualifiedName} interface, but it doesn't!");
        }

        if (cancellationToken == default)
        {
            var hostApplicationLifetime = context.ServiceProvider.GetService<IHostApplicationLifetime>();
            if (hostApplicationLifetime != null)
            {
                cancellationToken = hostApplicationLifetime.ApplicationStopping;
            }
        }

        await context.ServiceProvider
            .GetRequiredService<IBackgroundWorkerManager>()
            .AddAsync((IBackgroundWorker)context.ServiceProvider.GetRequiredService(workerType), cancellationToken);

        return context;
    }
}
