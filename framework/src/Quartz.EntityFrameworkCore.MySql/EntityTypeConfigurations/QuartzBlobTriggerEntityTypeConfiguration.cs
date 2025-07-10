namespace Quartz.EntityFrameworkCore.MySql.EntityTypeConfigurations;

public class QuartzBlobTriggerEntityTypeConfiguration(string? prefix) : IEntityTypeConfiguration<QuartzBlobTrigger>
{
    public void Configure(EntityTypeBuilder<QuartzBlobTrigger> builder)
    {
        builder.ToTable($"{prefix}BLOB_TRIGGERS");

        builder.HasKey(x => new { x.SchedulerName, x.TriggerName, x.TriggerGroup });

        builder.Property(x => x.SchedulerName)
            .HasColumnName("SCHED_NAME")
            .HasColumnType("varchar(120)")
            .IsRequired();

        builder.Property(x => x.TriggerName)
            .HasColumnName("TRIGGER_NAME")
            .HasColumnType("varchar(200)")
            .IsRequired();

        builder.Property(x => x.TriggerGroup)
            .HasColumnName("TRIGGER_GROUP")
            .HasColumnType("varchar(200)")
            .IsRequired();

        builder.Property(x => x.BlobData)
            .HasColumnName("BLOB_DATA")
            .HasColumnType("blob");

        builder.HasOne(x => x.Trigger)
            .WithMany(x => x.BlobTriggers)
            .HasForeignKey(x => new { x.SchedulerName, x.TriggerName, x.TriggerGroup })
            .OnDelete(DeleteBehavior.Cascade);
    }
}
