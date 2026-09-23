using System.Collections.Concurrent;

namespace Ran.BackgroundJob;

/// <summary>
/// Stores background jobs in the current process. Jobs do not survive an application restart.
/// </summary>
public sealed class InMemoryBackgroundJobStore : IBackgroundJobStore
{
    private readonly ConcurrentDictionary<string, BackgroundJobInfo> _jobs = new();

    public Task InsertAsync(
        BackgroundJobInfo jobInfo,
        CancellationToken cancellationToken = default
    )
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (!_jobs.TryAdd(jobInfo.Id, jobInfo))
        {
            throw new InvalidOperationException($"A background job with id '{jobInfo.Id}' exists.");
        }

        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<BackgroundJobInfo>> GetWaitingJobsAsync(
        int maxResultCount,
        DateTimeOffset now,
        CancellationToken cancellationToken = default
    )
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxResultCount);

        IReadOnlyList<BackgroundJobInfo> jobs = _jobs
            .Values.Where(job => !job.IsAbandoned && job.NextTryTime <= now)
            .OrderByDescending(job => job.Priority)
            .ThenBy(job => job.TryCount)
            .ThenBy(job => job.CreationTime)
            .Take(maxResultCount)
            .ToList();
        return Task.FromResult(jobs);
    }

    public Task UpdateAsync(
        BackgroundJobInfo jobInfo,
        CancellationToken cancellationToken = default
    )
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (!_jobs.TryGetValue(jobInfo.Id, out var currentJob))
        {
            throw new KeyNotFoundException($"Background job '{jobInfo.Id}' was not found.");
        }

        if (!_jobs.TryUpdate(jobInfo.Id, jobInfo, currentJob))
        {
            throw new InvalidOperationException(
                $"Background job '{jobInfo.Id}' was updated concurrently."
            );
        }

        return Task.CompletedTask;
    }

    public Task DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _ = _jobs.TryRemove(id, out _);
        return Task.CompletedTask;
    }
}
