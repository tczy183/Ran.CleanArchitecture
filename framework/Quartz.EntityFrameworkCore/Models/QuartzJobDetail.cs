// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace Quartz.EntityFrameworkCore.Models;

/// <summary>
/// Represents a job detail used by the Quartz scheduler.
/// </summary>
public class QuartzJobDetail
{
    /// <summary>
    /// Gets or sets the name of the scheduler.
    /// </summary>
    public string SchedulerName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the name of the job.
    /// </summary>
    public string JobName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the group of the job.
    /// </summary>
    public string JobGroup { get; set; } = null!;

    /// <summary>
    /// Gets or sets the description of the job.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the class name of the job.
    /// </summary>
    public string JobClassName { get; set; } = null!;

    /// <summary>
    /// Gets or sets a value indicating whether the job is durable.
    /// </summary>
    public bool IsDurable { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the job is non-concurrent.
    /// </summary>
    public bool IsNonConcurrent { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the job data should be updated.
    /// </summary>
    public bool IsUpdateData { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the job requests recovery.
    /// </summary>
    public bool RequestsRecovery { get; set; }

    /// <summary>
    /// Gets or sets the job data.
    /// </summary>
    public byte[]? JobData { get; set; } = null!;

    /// <summary>
    /// Gets or sets the collection of triggers associated with the job.
    /// </summary>
    public ICollection<QuartzTrigger> Triggers { get; set; } = null!;
}