using Microsoft.EntityFrameworkCore;

namespace Ran.BackgroundJob.EntityFrameworkCore;

public interface IBackgroundJobDbContext
{
    DbSet<BackgroundJobRecord> BackgroundJobs { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
