namespace Ran.BackgroundJob;

/// <summary>
/// Provides storage for background jobs. Replace the default in-memory implementation for durable execution.
/// </summary>
public interface IBackgroundJobStore
{
    Task InsertAsync(BackgroundJobInfo jobInfo, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<BackgroundJobInfo>> GetWaitingJobsAsync(
        int maxResultCount,
        DateTimeOffset now,
        CancellationToken cancellationToken = default
    );

    Task UpdateAsync(BackgroundJobInfo jobInfo, CancellationToken cancellationToken = default);

    Task DeleteAsync(string id, CancellationToken cancellationToken = default);
}
