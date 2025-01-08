// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace Quartz.EntityFrameworkCore.Models;

/// <summary>
/// Represents a trigger used by the Quartz scheduler.
/// </summary>
public class QuartzTrigger
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
    /// Gets or sets the name of the job.
    /// </summary>
    public string JobName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the group of the job.
    /// </summary>
    public string JobGroup { get; set; } = null!;

    /// <summary>
    /// Gets or sets the description of the trigger.
    /// </summary>
    public string? Description { get; set; } = null!;

    /// <summary>
    /// Gets or sets the next fire time.
    /// </summary>
    public long? NextFireTime { get; set; }

    /// <summary>
    /// Gets or sets the previous fire time.
    /// </summary>
    public long? PreviousFireTime { get; set; }

    /// <summary>
    /// Gets or sets the priority of the trigger.
    /// </summary>
    public int? Priority { get; set; }

    /// <summary>
    /// Gets or sets the state of the trigger.
    /// </summary>
    public string TriggerState { get; set; } = null!;

    /// <summary>
    /// Gets or sets the type of the trigger.
    /// </summary>
    public string TriggerType { get; set; } = null!;

    /// <summary>
    /// Gets or sets the start time of the trigger.
    /// </summary>
    public long StartTime { get; set; }

    /// <summary>
    /// Gets or sets the end time of the trigger.
    /// </summary>
    public long? EndTime { get; set; }

    /// <summary>
    /// Gets or sets the name of the calendar.
    /// </summary>
    public string? CalendarName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the misfire instruction.
    /// </summary>
    public short? MisfireInstruction { get; set; }

    /// <summary>
    /// Gets or sets the job data.
    /// </summary>
    public byte[]? JobData { get; set; }

    /// <summary>
    /// Gets or sets the associated job detail.
    /// </summary>
    public QuartzJobDetail JobDetail { get; set; } = null!;

    /// <summary>
    /// Gets or sets the collection of simple triggers associated with the trigger.
    /// </summary>
    public ICollection<QuartzSimpleTrigger> SimpleTriggers { get; set; } = null!;

    /// <summary>
    /// Gets or sets the collection of simple property triggers associated with the trigger.
    /// </summary>
    public ICollection<QuartzSimplePropertyTrigger> SimplePropertyTriggers { get; set; } = null!;

    /// <summary>
    /// Gets or sets the collection of cron triggers associated with the trigger.
    /// </summary>
    public ICollection<QuartzCronTrigger> CronTriggers { get; set; } = null!;

    /// <summary>
    /// Gets or sets the collection of blob triggers associated with the trigger.
    /// </summary>
    public ICollection<QuartzBlobTrigger> BlobTriggers { get; set; } = null!;
}