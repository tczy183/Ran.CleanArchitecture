namespace Quartz.EntityFrameworkCore;

public interface QuartzModelBuilder
{
    QuartzModelBuilder UseEntityTypeConfigurations(Action<EntityTypeConfigurationContext> entityTypeConfigurations);
}