namespace Ran.BackgroundJob;

/// <summary>
/// Queues background jobs for deferred execution.
/// </summary>
public interface IBackgroundJobManager
{
    /// <summary>
    /// Queues a background job.
    /// </summary>
    Task<string> EnqueueAsync<TArgs>(
        TArgs args,
        BackgroundJobPriority priority = BackgroundJobPriority.Normal,
        TimeSpan? delay = null,
        CancellationToken cancellationToken = default
    )
        where TArgs : notnull;
}
