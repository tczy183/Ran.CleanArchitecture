namespace Ran.BackgroundJob;

public sealed class BackgroundJobWorkerOptions
{
    public TimeSpan JobPollPeriod { get; set; } = TimeSpan.FromSeconds(5);

    public int MaxJobFetchCount { get; set; } = 1000;

    public TimeSpan DefaultFirstWaitDuration { get; set; } = TimeSpan.FromMinutes(1);

    public TimeSpan DefaultTimeout { get; set; } = TimeSpan.FromDays(2);

    public double DefaultWaitFactor { get; set; } = 2.0;
}
