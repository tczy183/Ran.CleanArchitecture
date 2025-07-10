// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace Quartz.EntityFrameworkCore.Models;

/// <summary>
/// Represents a cron trigger used by the Quartz scheduler.
/// </summary>
public class QuartzCronTrigger
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
    /// Gets or sets the cron expression.
    /// </summary>
    public string CronExpression { get; set; } = null!;

    /// <summary>
    /// Gets or sets the time zone ID.
    /// </summary>
    public string? TimeZoneId { get; set; }

    /// <summary>
    /// Gets or sets the associated trigger.
    /// </summary>
    public QuartzTrigger Trigger { get; set; } = null!;
}
