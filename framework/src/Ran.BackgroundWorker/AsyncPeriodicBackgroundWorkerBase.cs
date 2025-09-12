using Microsoft.Extensions.DependencyInjection;
using Ran.Core.Exceptions.Handling;
using Ran.Core.Exceptions.Handling.Abstracts;
using Ran.Core.Extensions.Logging;
using Ran.Core.Threading;

namespace Ran.BackgroundWorker;

public abstract class AsyncPeriodicBackgroundWorkerBase : BackgroundWorkerBase
{
    protected IServiceScopeFactory ServiceScopeFactory { get; }
    protected AsyncRanTimer RanTimer { get; }
    protected CancellationToken StartCancellationToken { get; set; }
    public int Period => RanTimer.Period;

    /// <summary>
    ///  CronExpression has high priority over Period.
    /// </summary>
    public string? CronExpression { get; protected set; }

    protected AsyncPeriodicBackgroundWorkerBase(
        AsyncRanTimer ranTimer,
        IServiceScopeFactory serviceScopeFactory
    )
    {
        ServiceScopeFactory = serviceScopeFactory;
        RanTimer = ranTimer;
        RanTimer.Elapsed = Timer_Elapsed;
    }

    public override async Task StartAsync(CancellationToken cancellationToken = default)
    {
        StartCancellationToken = cancellationToken;

        await base.StartAsync(cancellationToken);
        RanTimer.Start();
    }

    public override async Task StopAsync(CancellationToken cancellationToken = default)
    {
        RanTimer.Stop();
        await base.StopAsync(cancellationToken);
    }

    private async Task Timer_Elapsed(AsyncRanTimer ranTimer)
    {
        await DoWorkAsync(StartCancellationToken);
    }

    private async Task DoWorkAsync(CancellationToken cancellationToken = default)
    {
        using var scope = ServiceScopeFactory.CreateScope();
        try
        {
            await DoWorkAsync(
                new PeriodicBackgroundWorkerContext(scope.ServiceProvider, cancellationToken)
            );
        }
        catch (Exception ex)
        {
            await scope
                .ServiceProvider.GetRequiredService<IExceptionNotifier>()
                .NotifyAsync(new ExceptionNotificationContext(ex));

            Logger.LogException(ex);
        }
    }

    protected abstract Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext);
}
