using System.Text.Json;

namespace Ran.BackgroundJob;

public sealed class BackgroundJobOptions
{
    public bool IsJobExecutionEnabled { get; set; } = true;

    public JsonSerializerOptions SerializerOptions { get; } = new(JsonSerializerDefaults.Web);
}
