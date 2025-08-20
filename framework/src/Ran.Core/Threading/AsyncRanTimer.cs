using Microsoft.Extensions.Logging.Abstractions;
using Ran.Core.DependencyInjection.ServiceLifetimes;
using Ran.Core.Exceptions;
using Ran.Core.Exceptions.Handling.Abstracts;
using Ran.Core.Extensions.Logging;

namespace Ran.Core.Threading;

public class AsyncRanTimer : ITransientDependency, IAsyncDisposable
{
    /// <summary>
    /// This func is raised periodically according to Period of Timer.
    /// </summary>
    public Func<AsyncRanTimer, Task> Elapsed { get; set; }

    /// <summary>
    /// Task period of timer (as milliseconds).
    /// </summary>
    public int Period { get; set; }

    /// <summary>
    /// Indicates whether timer raises Elapsed event on Start method of Timer for once.
    /// Default: False.
    /// </summary>
    public bool RunOnStart { get; set; }

    public ILogger<AsyncRanTimer> Logger { get; set; }

    public IExceptionNotifier ExceptionNotifier { get; set; }

    private readonly Timer _taskTimer;
    private readonly object _lock = new object();
    private volatile bool _performingTasks;
    private volatile bool _isRunning;

    public AsyncRanTimer()
    {
        ExceptionNotifier = NullExceptionNotifier.Instance;
        Logger = NullLogger<AsyncRanTimer>.Instance;
        Elapsed = _ => Task.CompletedTask;

        _taskTimer = new Timer(TimerCallBack!, null, Timeout.Infinite, Timeout.Infinite);
    }

    public void Start()
    {
        if (Period <= 0)
        {
            throw new UserFriendlyException("Period should be set before starting the timer!");
        }

        lock (_lock)
        {
            _taskTimer.Change(RunOnStart ? 0 : Period, Timeout.Infinite);
            _isRunning = true;
        }
    }

    public void Stop()
    {
        lock (_lock)
        {
            _taskTimer.Change(Timeout.Infinite, Timeout.Infinite);
            while (_performingTasks)
            {
                Monitor.Wait(_taskTimer);
            }

            _isRunning = false;
        }
    }

    /// <summary>
    /// This method is called by _taskTimer.
    /// </summary>
    /// <param name="state">Not used argument</param>
    private void TimerCallBack(object state)
    {
        lock (_lock)
        {
            if (!_isRunning || _performingTasks)
            {
                return;
            }

            _taskTimer.Change(Timeout.Infinite, Timeout.Infinite);
            _performingTasks = true;
        }

        _ = Timer_Elapsed();
    }

    private async Task Timer_Elapsed()
    {
        try
        {
            await Elapsed(this);
        }
        catch (Exception ex)
        {
            Logger.LogException(ex);
            await ExceptionNotifier.NotifyAsync(ex);
        }
        finally
        {
            lock (_lock)
            {
                _performingTasks = false;
                if (_isRunning)
                {
                    _taskTimer.Change(Period, Timeout.Infinite);
                }

                Monitor.Pulse(_taskTimer);
            }
        }
    }

    public async ValueTask DisposeAsync()
    {
        await _taskTimer.DisposeAsync();
    }
}
