using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Ran.Core.BackgroundWorkers;
using Ran.Core.DependencyInjection;

namespace Ran.BackgroundWorker;

/// <summary>
/// Base class that can be used to implement <see cref="IBackgroundWorker"/>.
/// </summary>
public abstract class BackgroundWorkerBase : IBackgroundWorker
{
    //TODO: Add UOW, Localization and other useful properties..?

    public ILazyServiceProvider LazyServiceProvider { get; set; } = default!;

    public IServiceProvider ServiceProvider { get; set; } = default!;

    protected ILoggerFactory LoggerFactory => LazyServiceProvider.LazyGetRequiredService<ILoggerFactory>();

    protected ILogger Logger => LazyServiceProvider.LazyGetService<ILogger>(provider =>
        LoggerFactory?.CreateLogger(GetType().FullName!) ?? NullLogger.Instance);

    protected CancellationTokenSource StoppingTokenSource { get; set; }

    protected CancellationToken StoppingToken { get; set; }

    protected BackgroundWorkerBase()
    {
        StoppingTokenSource = new CancellationTokenSource();
        StoppingToken = StoppingTokenSource.Token;
    }

    public virtual Task StartAsync(CancellationToken cancellationToken = default)
    {
        Logger.LogDebug("Stopped background worker: {Message}", ToString());
        return Task.CompletedTask;
    }

    public virtual async Task StopAsync(CancellationToken cancellationToken = default)
    {
        Logger.LogDebug("Stopped background worker: {Message}", ToString());
        await StoppingTokenSource.CancelAsync();
        StoppingTokenSource.Dispose();
    }

    public override string ToString()
    {
        return GetType().FullName!;
    }
}
