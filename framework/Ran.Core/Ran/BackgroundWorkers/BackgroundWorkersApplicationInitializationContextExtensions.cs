using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Ran.Core.Ran;
using Ran.Core.Ran.BackgroundWorkers;
using Ran.Core.Ran.Exceptions;
using Ran.Core.Ran.Modularity;
using Ran.Core.Ran.Modularity.Abstractions;

namespace Ran.Core.BackgroundWorkers;

public static class BackgroundWorkersApplicationInitializationContextExtensions
{
    public static async Task<IApplicationInitializationContext> AddBackgroundWorkerAsync<TWorker>(
        [NotNull] this IApplicationInitializationContext context,
        CancellationToken cancellationToken = default
    )
        where TWorker : IBackgroundWorker
    {
        Check.NotNull(context, nameof(context));

        await context.AddBackgroundWorkerAsync(
            typeof(TWorker),
            cancellationToken: cancellationToken
        );

        return context;
    }

    public static async Task<IApplicationInitializationContext> AddBackgroundWorkerAsync(
        [NotNull] this IApplicationInitializationContext context,
        [NotNull] Type workerType,
        CancellationToken cancellationToken = default
    )
    {
        Check.NotNull(context, nameof(context));
        Check.NotNull(workerType, nameof(workerType));

        if (!workerType.IsAssignableTo<IBackgroundWorker>())
        {
            throw new RanException(
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
