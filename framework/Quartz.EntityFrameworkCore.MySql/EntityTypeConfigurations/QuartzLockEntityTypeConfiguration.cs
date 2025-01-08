namespace Quartz.EntityFrameworkCore.MySql.EntityTypeConfigurations;

public class QuartzLockEntityTypeConfiguration(string? prefix) : IEntityTypeConfiguration<QuartzLock>
{
    public void Configure(EntityTypeBuilder<QuartzLock> builder)
    {
        builder.ToTable($"{prefix}LOCKS");

        builder.HasKey(x => new { x.SchedulerName, x.LockName });

        builder.Property(x => x.SchedulerName)
            .HasColumnName("SCHED_NAME")
            .HasColumnType("varchar(120)")
            .IsRequired();

        builder.Property(x => x.LockName)
            .HasColumnName("LOCK_NAME")
            .HasColumnType("varchar(40)")
            .IsRequired();
    }
}