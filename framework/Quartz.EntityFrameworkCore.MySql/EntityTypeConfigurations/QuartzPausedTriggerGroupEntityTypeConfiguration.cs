namespace Quartz.EntityFrameworkCore.MySql.EntityTypeConfigurations;

public class QuartzPausedTriggerGroupEntityTypeConfiguration(string? prefix)
    : IEntityTypeConfiguration<QuartzPausedTriggerGroup>
{
    public void Configure(EntityTypeBuilder<QuartzPausedTriggerGroup> builder)
    {
        builder.ToTable($"{prefix}PAUSED_TRIGGER_GRPS");

        builder.HasKey(x => new { x.SchedulerName, x.TriggerGroup });

        builder.Property(x => x.SchedulerName)
            .HasColumnName("SCHED_NAME")
            .HasColumnType("varchar(120)")
            .IsRequired();

        builder.Property(x => x.TriggerGroup)
            .HasColumnName("TRIGGER_GROUP")
            .HasColumnType("varchar(200)")
            .IsRequired();
    }
}