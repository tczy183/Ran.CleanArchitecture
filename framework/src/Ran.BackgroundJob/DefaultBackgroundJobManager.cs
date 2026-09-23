using System.Text.Json;
using Microsoft.Extensions.Options;

namespace Ran.BackgroundJob;

public sealed class DefaultBackgroundJobManager(
    IBackgroundJobStore store,
    IOptions<BackgroundJobOptions> options,
    TimeProvider timeProvider
) : IBackgroundJobManager
{
    public async Task<string> EnqueueAsync<TArgs>(
        TArgs args,
        BackgroundJobPriority priority = BackgroundJobPriority.Normal,
        TimeSpan? delay = null,
        CancellationToken cancellationToken = default
    )
        where TArgs : notnull
    {
        if (delay < TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(delay), "Delay cannot be negative.");
        }

        var argsType = typeof(TArgs);
        var now = timeProvider.GetUtcNow();
        var jobInfo = new BackgroundJobInfo
        {
            Id = Guid.NewGuid().ToString("N"),
            JobName = argsType.FullName ?? argsType.Name,
            ArgsType =
                argsType.AssemblyQualifiedName
                ?? throw new InvalidOperationException(
                    "The job argument type has no assembly name."
                ),
            ArgsJson = JsonSerializer.Serialize(args, options.Value.SerializerOptions),
            Priority = priority,
            CreationTime = now,
            NextTryTime = now + (delay ?? TimeSpan.Zero),
        };

        await store.InsertAsync(jobInfo, cancellationToken);
        return jobInfo.Id;
    }
}
