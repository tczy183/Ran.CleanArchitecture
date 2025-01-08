// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace Quartz.EntityFrameworkCore.Models;

/// <summary>
/// Represents a lock used by the Quartz scheduler.
/// </summary>
public class QuartzLock
{
    /// <summary>
    /// Gets or sets the name of the scheduler.
    /// </summary>
    public string SchedulerName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the name of the lock.
    /// </summary>
    public string LockName { get; set; } = null!;
}