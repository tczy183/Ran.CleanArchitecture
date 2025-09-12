using Microsoft.Extensions.DependencyInjection;
using Ran.Core.Exceptions.Handling;
using Ran.Core.Exceptions.Handling.Abstracts;
using Ran.Core.Extensions.Logging;
using Ran.Core.Threading;
using Ran.Core.Utils.Threading;

namespace Ran.BackgroundWorker;

/// <summary>
/// Extends <see cref="BackgroundWorkerBase"/> to add a periodic running Timer.
/// </summary>
public abstract class PeriodicBackgroundWorkerBase : BackgroundWorkerBase
{
    protected IServiceScopeFactory ServiceScopeFactory { get; }
    protected RanTimer RanTimer { get; }
    public int Period => RanTimer.Period;

    /// <summary>
    ///  CronExpression has high priority over Period.
    /// </summary>
    public string? CronExpression { get; protected set; }

    protected PeriodicBackgroundWorkerBase(
        RanTimer ranTimer,
        IServiceScopeFactory serviceScopeFactory
    )
    {
        ServiceScopeFactory = serviceScopeFactory;
        RanTimer = ranTimer;
        RanTimer.Elapsed += Timer_Elapsed;
    }

    public override async Task StartAsync(CancellationToken cancellationToken = default)
    {
        await base.StartAsync(cancellationToken);
        RanTimer.Start();
    }

    public override async Task StopAsync(CancellationToken cancellationToken = default)
    {
        RanTimer.Stop();
        await base.StopAsync(cancellationToken);
    }

    private void Timer_Elapsed(object? sender, System.EventArgs e)
    {
        using var scope = ServiceScopeFactory.CreateScope();
        try
        {
            DoWork(new PeriodicBackgroundWorkerContext(scope.ServiceProvider));
        }
        catch (Exception ex)
        {
            var exceptionNotifier = scope.ServiceProvider.GetRequiredService<IExceptionNotifier>();
            AsyncHelper.RunSync(() =>
                exceptionNotifier.NotifyAsync(new ExceptionNotificationContext(ex))
            );

            Logger.LogException(ex);
        }
    }

    /// <summary>
    /// Periodic works should be done by implementing this method.
    /// </summary>
    protected abstract void DoWork(PeriodicBackgroundWorkerContext workerContext);
}
