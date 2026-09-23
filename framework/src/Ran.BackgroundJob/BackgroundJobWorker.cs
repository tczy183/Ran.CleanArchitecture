using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ran.BackgroundWorker;
using Ran.Core.Threading;

namespace Ran.BackgroundJob;

public sealed class BackgroundJobWorker : AsyncPeriodicBackgroundWorkerBase
{
    private readonly BackgroundJobWorkerOptions _options;
    private readonly TimeProvider _timeProvider;
    private readonly ILogger<BackgroundJobWorker> _logger;

    public BackgroundJobWorker(
        AsyncRanTimer timer,
        IServiceScopeFactory serviceScopeFactory,
        IOptions<BackgroundJobWorkerOptions> options,
        TimeProvider timeProvider,
        ILogger<BackgroundJobWorker> logger
    )
        : base(timer, serviceScopeFactory)
    {
        _options = options.Value;
        _timeProvider = timeProvider;
        _logger = logger;

        ArgumentOutOfRangeException.ThrowIfLessThan(
            _options.JobPollPeriod,
            TimeSpan.FromMilliseconds(1)
        );
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(_options.MaxJobFetchCount);
        ArgumentOutOfRangeException.ThrowIfLessThan(
            _options.DefaultFirstWaitDuration,
            TimeSpan.Zero
        );
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(_options.DefaultTimeout, TimeSpan.Zero);
        ArgumentOutOfRangeException.ThrowIfLessThan(_options.DefaultWaitFactor, 1D);
        timer.Period = checked((int)_options.JobPollPeriod.TotalMilliseconds);
        timer.RunOnStart = true;
    }

    protected override async Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
    {
        var now = _timeProvider.GetUtcNow();
        var store = workerContext.ServiceProvider.GetRequiredService<IBackgroundJobStore>();
        var jobs = await store.GetWaitingJobsAsync(
            _options.MaxJobFetchCount,
            now,
            workerContext.CancellationToken
        );

        foreach (var job in jobs)
        {
            await ExecuteJobAsync(job, workerContext, store, now);
        }
    }

    private async Task ExecuteJobAsync(
        BackgroundJobInfo job,
        PeriodicBackgroundWorkerContext workerContext,
        IBackgroundJobStore store,
        DateTimeOffset now
    )
    {
        if (now - job.CreationTime >= _options.DefaultTimeout)
        {
            await store.UpdateAsync(
                job with
                {
                    IsAbandoned = true,
                    LastError = "The job timed out.",
                },
                workerContext.CancellationToken
            );
            return;
        }

        try
        {
            var executor =
                workerContext.ServiceProvider.GetRequiredService<IBackgroundJobExecutor>();
            await executor.ExecuteAsync(
                job,
                workerContext.ServiceProvider,
                workerContext.CancellationToken
            );
            await store.DeleteAsync(job.Id, workerContext.CancellationToken);
        }
        catch (OperationCanceledException)
            when (workerContext.CancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception exception)
        {
            var tryCount = job.TryCount + 1;
            var retryDelay = CalculateRetryDelay(tryCount);
            var failedJob = job with
            {
                TryCount = tryCount,
                LastTryTime = now,
                NextTryTime = now + retryDelay,
                LastError = exception.ToString(),
            };
            await store.UpdateAsync(failedJob, workerContext.CancellationToken);

            if (_logger.IsEnabled(LogLevel.Error))
            {
                _logger.LogError(
                    exception,
                    "Background job {JobId} ({JobName}) failed. Retry {TryCount} is scheduled at {NextTryTime}.",
                    job.Id,
                    job.JobName,
                    tryCount,
                    failedJob.NextTryTime
                );
            }
        }
    }

    private TimeSpan CalculateRetryDelay(int tryCount)
    {
        var ticks =
            _options.DefaultFirstWaitDuration.Ticks
            * Math.Pow(_options.DefaultWaitFactor, Math.Max(0, tryCount - 1));
        return double.IsFinite(ticks) && ticks < TimeSpan.MaxValue.Ticks
            ? TimeSpan.FromTicks((long)ticks)
            : TimeSpan.MaxValue;
    }
}
