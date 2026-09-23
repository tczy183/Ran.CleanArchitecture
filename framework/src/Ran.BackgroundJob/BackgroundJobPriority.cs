namespace Ran.BackgroundJob;

/// <summary>
/// Defines the relative execution priority of a background job.
/// </summary>
public enum BackgroundJobPriority
{
    Low = -20,
    BelowNormal = -10,
    Normal = 0,
    AboveNormal = 10,
    High = 20,
}
