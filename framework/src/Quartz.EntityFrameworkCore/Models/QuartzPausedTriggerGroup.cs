// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace Quartz.EntityFrameworkCore.Models;

/// <summary>
/// Represents a paused trigger group used by the Quartz scheduler.
/// </summary>
public class QuartzPausedTriggerGroup
{
    /// <summary>
    /// Gets or sets the name of the scheduler.
    /// </summary>
    public string SchedulerName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the group of the trigger.
    /// </summary>
    public string TriggerGroup { get; set; } = null!;
}
