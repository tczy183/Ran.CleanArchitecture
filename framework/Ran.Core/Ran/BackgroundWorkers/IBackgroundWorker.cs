using Ran.Core.Ran.DependencyInjection;
using Ran.DenpendencyInjection;

namespace Ran.Core.Ran.BackgroundWorkers;

/// <summary>
/// Interface for a worker (thread) that runs on background to perform some tasks.
/// </summary>
public interface IBackgroundWorker : IRunnable, ISingletonDependency { }
