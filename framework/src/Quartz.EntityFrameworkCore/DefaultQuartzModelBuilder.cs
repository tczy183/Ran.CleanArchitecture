namespace Quartz.EntityFrameworkCore;

public class DefaultQuartzModelBuilder(QuartzModel model) : IQuartzModelBuilder
{
    public IQuartzModelBuilder UseEntityTypeConfigurations(
        Action<EntityTypeConfigurationContext> entityTypeConfigurations
    )
    {
        model.EntityTypeConfigurations = entityTypeConfigurations;

        return this;
    }
}
