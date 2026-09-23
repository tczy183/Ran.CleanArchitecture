namespace Ran.BackgroundJob.EntityFrameworkCore;

public static class BackgroundJobDbProperties
{
    public const string ConnectionStringName = "BackgroundJobs";

    public static string TablePrefix { get; set; } = "Ran";

    public static string? Schema { get; set; }
}
