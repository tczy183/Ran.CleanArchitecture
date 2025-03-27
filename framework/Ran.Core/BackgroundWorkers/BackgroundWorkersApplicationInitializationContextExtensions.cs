using Ran.Core.Application;
using Ran.Core.Exceptions;
using Ran.Core.Utils.System;

namespace Ran.Core.BackgroundWorkers;

public static class BackgroundWorkersApplicationInitializationContextExtensions
{
    public static async Task<ApplicationInitializationContext> AddBackgroundWorkerAsync<TWorker>(
        [NotNull] this ApplicationInitializationContext context,
        CancellationToken cancellationToken = default
    )
        where TWorker : IBackgroundWorker
    {
        CheckHelper.NotNull(context, nameof(context));

        await context.AddBackgroundWorkerAsync(
            typeof(TWorker),
            cancellationToken: cancellationToken
        );

        return context;
    }

    public static async Task<ApplicationInitializationContext> AddBackgroundWorkerAsync(
        [NotNull] this ApplicationInitializationContext context,
        [NotNull] Type workerType,
        CancellationToken cancellationToken = default
    )
    {
        CheckHelper.NotNull(context, nameof(context));
        CheckHelper.NotNull(workerType, nameof(workerType));

        if (!workerType.IsAssignableTo<IBackgroundWorker>())
        {
            throw new UserFriendlyException(
                $"Given type ({workerType.AssemblyQualifiedName}) must implement the {typeof(IBackgroundWorker).AssemblyQualifiedName} interface, but it doesn't!"
            );
        }

        await context
            .ServiceProvider.GetRequiredService<IBackgroundWorkerManager>()
            .AddAsync(
                (IBackgroundWorker)context.ServiceProvider.GetRequiredService(workerType),
                cancellationToken
            );

        return context;
    }
}
