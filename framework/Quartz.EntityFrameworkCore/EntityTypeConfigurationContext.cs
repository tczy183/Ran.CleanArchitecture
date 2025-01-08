namespace Quartz.EntityFrameworkCore;

public record struct EntityTypeConfigurationContext(ModelBuilder modelBuilder)
{
    public ModelBuilder ModelBuilder { get; } = modelBuilder;
}