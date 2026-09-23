namespace Ran.BackgroundJob.EntityFrameworkCore;

public sealed class BackgroundJobRecord
{
    public required string Id { get; set; }

    public required string JobName { get; set; }

    public required string ArgsType { get; set; }

    public required string ArgsJson { get; set; }

    public BackgroundJobPriority Priority { get; set; }

    public DateTimeOffset CreationTime { get; set; }

    public DateTimeOffset NextTryTime { get; set; }

    public DateTimeOffset? LastTryTime { get; set; }

    public int TryCount { get; set; }

    public bool IsAbandoned { get; set; }

    public string? LastError { get; set; }
}
