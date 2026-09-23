using Microsoft.EntityFrameworkCore;

namespace Ran.BackgroundJob.EntityFrameworkCore;

public static class BackgroundJobModelBuilderExtensions
{
    public static ModelBuilder ConfigureBackgroundJobs(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BackgroundJobRecord>(builder =>
        {
            builder.ToTable(
                BackgroundJobDbProperties.TablePrefix + "BackgroundJobs",
                BackgroundJobDbProperties.Schema
            );
            builder.HasKey(job => job.Id);
            builder.Property(job => job.Id).HasMaxLength(64);
            builder.Property(job => job.JobName).HasMaxLength(256).IsRequired();
            builder.Property(job => job.ArgsType).HasMaxLength(1024).IsRequired();
            builder.Property(job => job.ArgsJson).IsRequired();
            builder.Property(job => job.LastError);
            builder.HasIndex(job => new
            {
                job.IsAbandoned,
                job.NextTryTime,
                job.Priority,
            });
        });

        return modelBuilder;
    }
}
