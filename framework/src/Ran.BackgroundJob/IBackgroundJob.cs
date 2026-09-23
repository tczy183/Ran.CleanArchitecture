using Ran.Core.DependencyInjection.ServiceLifetimes;

namespace Ran.BackgroundJob;

/// <summary>
/// Handles a background job with strongly typed arguments.
/// </summary>
/// <typeparam name="TArgs">The job argument type.</typeparam>
public interface IBackgroundJob<in TArgs> : ITransientDependency
    where TArgs : notnull
{
    /// <summary>
    /// Executes the background job.
    /// </summary>
    Task ExecuteAsync(TArgs args, CancellationToken cancellationToken = default);
}
