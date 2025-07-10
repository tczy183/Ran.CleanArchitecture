namespace Quartz.EntityFrameworkCore;

public static class ModelBuilderExtensions
{
    public static ModelBuilder AddQuartz(
        this ModelBuilder modelBuilder,
        Action<IQuartzModelBuilder>? configure)
    {
        var model = new QuartzModel();
        configure?.Invoke(new DefaultQuartzModelBuilder(model));

        if (model.EntityTypeConfigurations is null)
        {
            throw new InvalidOperationException("No database provider");
        }

        model.EntityTypeConfigurations.Invoke(new EntityTypeConfigurationContext(modelBuilder));

        return modelBuilder;
    }
}
