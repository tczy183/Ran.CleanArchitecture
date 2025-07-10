// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace Quartz.EntityFrameworkCore.Models;

/// <summary>
/// Represents a simple property trigger used by the Quartz scheduler.
/// </summary>
public class QuartzSimplePropertyTrigger
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
    /// Gets or sets the first string property.
    /// </summary>
    public string? StringProperty1 { get; set; }

    /// <summary>
    /// Gets or sets the second string property.
    /// </summary>
    public string? StringProperty2 { get; set; }

    /// <summary>
    /// Gets or sets the third string property.
    /// </summary>
    public string? StringProperty3 { get; set; }

    /// <summary>
    /// Gets or sets the first integer property.
    /// </summary>
    public int? IntegerProperty1 { get; set; }

    /// <summary>
    /// Gets or sets the second integer property.
    /// </summary>
    public int? IntegerProperty2 { get; set; }

    /// <summary>
    /// Gets or sets the first long property.
    /// </summary>
    public long? LongProperty1 { get; set; }

    /// <summary>
    /// Gets or sets the second long property.
    /// </summary>
    public long? LongProperty2 { get; set; }

    /// <summary>
    /// Gets or sets the first decimal property.
    /// </summary>
    public decimal? DecimalProperty1 { get; set; }

    /// <summary>
    /// Gets or sets the second decimal property.
    /// </summary>
    public decimal? DecimalProperty2 { get; set; }

    /// <summary>
    /// Gets or sets the first boolean property.
    /// </summary>
    public bool? BooleanProperty1 { get; set; }

    /// <summary>
    /// Gets or sets the second boolean property.
    /// </summary>
    public bool? BooleanProperty2 { get; set; }

    /// <summary>
    /// Gets or sets the time zone ID.
    /// </summary>
    public string? TimeZoneId { get; set; }

    /// <summary>
    /// Gets or sets the associated trigger.
    /// </summary>
    public QuartzTrigger Trigger { get; set; } = null!;
}
