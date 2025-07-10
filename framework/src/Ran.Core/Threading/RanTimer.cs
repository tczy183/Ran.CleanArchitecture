using Microsoft.Extensions.Logging.Abstractions;
using Ran.Core.DependencyInjection.ServiceLifetimes;
using Ran.Core.Exceptions;
using Ran.Core.Exceptions.Handling.Abstracts;
using Ran.Core.Extensions.Logging;
using Ran.Core.Utils.Threading;

namespace Ran.Core.Threading;

/// <summary>
/// A robust timer implementation that ensures no overlapping occurs. It waits exactly specified <see cref="Period"/> between ticks.
/// </summary>
public class RanTimer : ITransientDependency, IDisposable
{
    /// <summary>
    /// This event is raised periodically according to Period of Timer.
    /// </summary>
    public event EventHandler Elapsed;

    /// <summary>
    /// Task period of timer (as milliseconds).
    /// </summary>
    public int Period { get; set; }

    /// <summary>
    /// Indicates whether timer raises Elapsed event on Start method of Timer for once.
    /// Default: False.
    /// </summary>
    public bool RunOnStart { get; set; }

    public ILogger<RanTimer> Logger { get; set; }

    public IExceptionNotifier ExceptionNotifier { get; set; }

    private readonly Timer _taskTimer;
    private readonly object _lock = new object();
    private volatile bool _performingTasks;
    private volatile bool _isRunning;

    public RanTimer()
    {
        ExceptionNotifier = NullExceptionNotifier.Instance;
        Logger = NullLogger<RanTimer>.Instance;


        _taskTimer = new Timer(
            TimerCallBack!,
            null,
            Timeout.Infinite,
            Timeout.Infinite
        );
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

        try
        {
            Elapsed.Invoke(this, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            Logger.LogException(ex);
            AsyncHelper.RunSync(() => ExceptionNotifier.NotifyAsync(ex));
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

    public void Dispose()
    {
        _taskTimer.Dispose();
    }
}
