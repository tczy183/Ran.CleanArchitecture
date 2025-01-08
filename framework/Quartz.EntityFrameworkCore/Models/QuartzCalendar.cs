// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace Quartz.EntityFrameworkCore.Models;

/// <summary>
/// Represents a calendar used by the Quartz scheduler.
/// </summary>
public class QuartzCalendar
{
    /// <summary>
    /// Gets or sets the name of the scheduler.
    /// </summary>
    public string SchedulerName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the name of the calendar.
    /// </summary>
    public string CalendarName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the calendar data.
    /// </summary>
    public byte[] Calendar { get; set; } = null!;
}