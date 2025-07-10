// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace Quartz.EntityFrameworkCore.Models;

/// <summary>
/// Represents a fired trigger used by the Quartz scheduler.
/// </summary>
public class QuartzFiredTrigger
{
    /// <summary>
    /// Gets or sets the name of the scheduler.
    /// </summary>
    public string SchedulerName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the entry ID.
    /// </summary>
    public string EntryId { get; set; } = null!;

    /// <summary>
    /// Gets or sets the name of the trigger.
    /// </summary>
    public string TriggerName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the group of the trigger.
    /// </summary>
    public string TriggerGroup { get; set; } = null!;

    /// <summary>
    /// Gets or sets the name of the instance.
    /// </summary>
    public string InstanceName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the fired time.
    /// </summary>
    public long FiredTime { get; set; }

    /// <summary>
    /// Gets or sets the scheduled time.
    /// </summary>
    public long ScheduledTime { get; set; }

    /// <summary>
    /// Gets or sets the priority.
    /// </summary>
    public int Priority { get; set; }

    /// <summary>
    /// Gets or sets the state.
    /// </summary>
    public string State { get; set; } = null!;

    /// <summary>
    /// Gets or sets the name of the job.
    /// </summary>
    public string? JobName { get; set; }

    /// <summary>
    /// Gets or sets the group of the job.
    /// </summary>
    public string? JobGroup { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the trigger is non-concurrent.
    /// </summary>
    public bool IsNonConcurrent { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the trigger requests recovery.
    /// </summary>
    public bool? RequestsRecovery { get; set; }
}
