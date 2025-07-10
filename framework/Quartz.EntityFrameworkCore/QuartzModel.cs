namespace Quartz.EntityFrameworkCore;

public class QuartzModel
{
    public string? Prefix { get; set; }
    public string? Schema { get; set; }
    public Action<EntityTypeConfigurationContext>? EntityTypeConfigurations { get; set; }
}
