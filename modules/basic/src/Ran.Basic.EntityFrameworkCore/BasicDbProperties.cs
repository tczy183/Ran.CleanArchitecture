namespace Ran.Basic.EntityFrameworkCore;

public static class BasicDbProperties
{
    public const string ConnectionStringName = "Basic";

    public static string TablePrefix { get; set; } = "Ran";

    public static string? Schema { get; set; }
}
