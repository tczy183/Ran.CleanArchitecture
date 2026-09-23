using Ran.Core.BackgroundWorkers;
using Ran.Core.DependencyInjection.ServiceLifetimes;

namespace Ran.BackgroundWorker;

/// <summary>
/// Implements <see cref="IBackgroundWorkerManager"/>.
/// </summary>
public class BackgroundWorkerManager : IBackgroundWorkerManager, ISingletonDependency, IDisposable
{
    protected bool IsRunning { get; private set; }

    private bool _isDisposed;
    private readonly object _sync = new();

    private readonly List<IBackgroundWorker> _backgroundWorkers;

    /// <summary>
    /// Initializes a new instance of the <see cref="BackgroundWorkerManager"/> class.
    /// </summary>
    public BackgroundWorkerManager()
    {
        _backgroundWorkers = [];
    }

    public virtual async Task AddAsync(
        IBackgroundWorker worker,
        CancellationToken cancellationToken = default
    )
    {
        bool shouldStart;
        lock (_sync)
        {
            ObjectDisposedException.ThrowIf(_isDisposed, this);
            if (_backgroundWorkers.Contains(worker))
            {
                return;
            }

            _backgroundWorkers.Add(worker);
            shouldStart = IsRunning;
        }

        if (shouldStart)
        {
            await worker.StartAsync(cancellationToken);
        }
    }

    public virtual void Dispose()
    {
        List<IBackgroundWorker> workers;
        lock (_sync)
        {
            if (_isDisposed)
            {
                return;
            }

            _isDisposed = true;
            IsRunning = false;
            workers = [.. _backgroundWorkers];
            _backgroundWorkers.Clear();
        }

        foreach (var worker in workers)
        {
            if (worker is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }

    public virtual async Task StartAsync(CancellationToken cancellationToken = default)
    {
        List<IBackgroundWorker> workers;
        lock (_sync)
        {
            ObjectDisposedException.ThrowIf(_isDisposed, this);
            if (IsRunning)
            {
                return;
            }

            IsRunning = true;
            workers = [.. _backgroundWorkers];
        }

        foreach (var worker in workers)
        {
            await worker.StartAsync(cancellationToken);
        }
    }

    public virtual async Task StopAsync(CancellationToken cancellationToken = default)
    {
        List<IBackgroundWorker> workers;
        lock (_sync)
        {
            if (!IsRunning)
            {
                return;
            }

            IsRunning = false;
            workers = [.. _backgroundWorkers];
        }

        for (var index = workers.Count - 1; index >= 0; index--)
        {
            await workers[index].StopAsync(cancellationToken);
        }
    }
}
