namespace Ran.BackgroundJob;

/// <summary>
/// Represents the provider-neutral persisted state of a background job.
/// </summary>
public sealed record BackgroundJobInfo
{
    public required string Id { get; init; }

    public required string JobName { get; init; }

    public required string ArgsType { get; init; }

    public required string ArgsJson { get; init; }

    public BackgroundJobPriority Priority { get; init; }

    public required DateTimeOffset CreationTime { get; init; }

    public required DateTimeOffset NextTryTime { get; init; }

    public DateTimeOffset? LastTryTime { get; init; }

    public int TryCount { get; init; }

    public bool IsAbandoned { get; init; }

    public string? LastError { get; init; }
}
