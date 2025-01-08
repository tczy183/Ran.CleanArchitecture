namespace Quartz.EntityFrameworkCore;

public class DefaultQuartzModelBuilder(QuartzModel model) : QuartzModelBuilder
{
    public QuartzModelBuilder UseEntityTypeConfigurations(
        Action<EntityTypeConfigurationContext> entityTypeConfigurations)
    {
        model.EntityTypeConfigurations = entityTypeConfigurations;

        return this;
    }
}