namespace Ran.BackgroundJob;

public interface IBackgroundJobExecutor
{
    Task ExecuteAsync(
        BackgroundJobInfo jobInfo,
        IServiceProvider serviceProvider,
        CancellationToken cancellationToken = default
    );
}
