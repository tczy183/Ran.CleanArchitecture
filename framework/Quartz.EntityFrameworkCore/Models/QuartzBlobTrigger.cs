namespace Quartz.EntityFrameworkCore.Models;

/// <summary>
/// Represents a blob trigger used by the Quartz scheduler.
/// </summary>
public class QuartzBlobTrigger
{
    /// <summary>
    /// Gets or sets the name of the scheduler.
    /// </summary>
    public string SchedulerName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the name of the trigger.
    /// </summary>
    public string TriggerName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the group of the trigger.
    /// </summary>
    public string TriggerGroup { get; set; } = null!;

    /// <summary>
    /// Gets or sets the blob data associated with the trigger.
    /// </summary>
    public byte[]? BlobData { get; set; }

    /// <summary>
    /// Gets or sets the associated trigger.
    /// </summary>
    public QuartzTrigger Trigger { get; set; } = null!;
}
