// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace Quartz.EntityFrameworkCore.Models;

/// <summary>
/// Represents a simple trigger used by the Quartz scheduler.
/// </summary>
public class QuartzSimpleTrigger
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
    /// Gets or sets the repeat count.
    /// </summary>
    public long RepeatCount { get; set; }

    /// <summary>
    /// Gets or sets the repeat interval.
    /// </summary>
    public long RepeatInterval { get; set; }

    /// <summary>
    /// Gets or sets the number of times the trigger has been fired.
    /// </summary>
    public long TimesTriggered { get; set; }

    /// <summary>
    /// Gets or sets the associated trigger.
    /// </summary>
    public QuartzTrigger Trigger { get; set; } = null!;
}