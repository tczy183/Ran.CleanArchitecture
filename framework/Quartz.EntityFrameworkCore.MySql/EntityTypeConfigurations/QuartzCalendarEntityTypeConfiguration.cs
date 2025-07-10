
namespace Quartz.EntityFrameworkCore.MySql.EntityTypeConfigurations;

public class QuartzCalendarEntityTypeConfiguration(string? prefix) : IEntityTypeConfiguration<QuartzCalendar>
{
    public void Configure(EntityTypeBuilder<QuartzCalendar> builder)
    {
        builder.ToTable($"{prefix}CALENDARS");

        builder.HasKey(x => new { x.SchedulerName, x.CalendarName });

        builder.Property(x => x.SchedulerName)
            .HasColumnName("SCHED_NAME")
            .HasColumnType("varchar(120)")
            .IsRequired();

        builder.Property(x => x.CalendarName)
            .HasColumnName("CALENDAR_NAME")
            .HasColumnType("varchar(200)")
            .IsRequired();

        builder.Property(x => x.Calendar)
            .HasColumnName("CALENDAR")
            .HasColumnType("blob")
            .IsRequired();
    }
}
