namespace Quartz.EntityFrameworkCore;

public interface IQuartzModelBuilder
{
    IQuartzModelBuilder UseEntityTypeConfigurations(Action<EntityTypeConfigurationContext> entityTypeConfigurations);
}
