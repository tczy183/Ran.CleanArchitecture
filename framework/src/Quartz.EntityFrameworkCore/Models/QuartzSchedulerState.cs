// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace Quartz.EntityFrameworkCore.Models;

/// <summary>
/// Represents the state of a Quartz scheduler.
/// </summary>
public class QuartzSchedulerState
{
    /// <summary>
    /// Gets or sets the name of the scheduler.
    /// </summary>
    public string SchedulerName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the name of the instance.
    /// </summary>
    public string InstanceName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the last check-in time.
    /// </summary>
    public long LastCheckInTime { get; set; }

    /// <summary>
    /// Gets or sets the check-in interval.
    /// </summary>
    public long CheckInInterval { get; set; }
}
