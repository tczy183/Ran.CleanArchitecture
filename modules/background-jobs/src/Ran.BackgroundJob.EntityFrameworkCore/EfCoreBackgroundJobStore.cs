using Microsoft.EntityFrameworkCore;

namespace Ran.BackgroundJob.EntityFrameworkCore;

public sealed class EfCoreBackgroundJobStore<TDbContext>(TDbContext dbContext) : IBackgroundJobStore
    where TDbContext : DbContext, IBackgroundJobDbContext
{
    public async Task InsertAsync(
        BackgroundJobInfo jobInfo,
        CancellationToken cancellationToken = default
    )
    {
        await dbContext.BackgroundJobs.AddAsync(ToRecord(jobInfo), cancellationToken);
        _ = await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<BackgroundJobInfo>> GetWaitingJobsAsync(
        int maxResultCount,
        DateTimeOffset now,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxResultCount);

        var records = await dbContext
            .BackgroundJobs.AsNoTracking()
            .Where(job => !job.IsAbandoned && job.NextTryTime <= now)
            .OrderByDescending(job => job.Priority)
            .ThenBy(job => job.TryCount)
            .ThenBy(job => job.CreationTime)
            .Take(maxResultCount)
            .ToListAsync(cancellationToken);
        return records.Select(ToInfo).ToList();
    }

    public async Task UpdateAsync(
        BackgroundJobInfo jobInfo,
        CancellationToken cancellationToken = default
    )
    {
        dbContext.BackgroundJobs.Update(ToRecord(jobInfo));
        _ = await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        _ = await dbContext
            .BackgroundJobs.Where(job => job.Id == id)
            .ExecuteDeleteAsync(cancellationToken);
    }

    private static BackgroundJobRecord ToRecord(BackgroundJobInfo jobInfo)
    {
        return new BackgroundJobRecord
        {
            Id = jobInfo.Id,
            JobName = jobInfo.JobName,
            ArgsType = jobInfo.ArgsType,
            ArgsJson = jobInfo.ArgsJson,
            Priority = jobInfo.Priority,
            CreationTime = jobInfo.CreationTime,
            NextTryTime = jobInfo.NextTryTime,
            LastTryTime = jobInfo.LastTryTime,
            TryCount = jobInfo.TryCount,
            IsAbandoned = jobInfo.IsAbandoned,
            LastError = jobInfo.LastError,
        };
    }

    private static BackgroundJobInfo ToInfo(BackgroundJobRecord record)
    {
        return new BackgroundJobInfo
        {
            Id = record.Id,
            JobName = record.JobName,
            ArgsType = record.ArgsType,
            ArgsJson = record.ArgsJson,
            Priority = record.Priority,
            CreationTime = record.CreationTime,
            NextTryTime = record.NextTryTime,
            LastTryTime = record.LastTryTime,
            TryCount = record.TryCount,
            IsAbandoned = record.IsAbandoned,
            LastError = record.LastError,
        };
    }
}
