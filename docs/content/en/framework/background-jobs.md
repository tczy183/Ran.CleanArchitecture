# Background Jobs

`Ran.BackgroundJob` provides storage-neutral background job abstractions inspired by ABP's typed arguments, priorities, delayed execution, and exponential retry behavior. Its default store is process-local and is intended for development or non-durable single-instance workloads.

## Define and enqueue a job

A handler implementing `IBackgroundJob<TArgs>` is registered as a transient service by the existing conventions:

```csharp
public sealed record SendWelcomeEmailArgs(Guid UserId, string Email);

public sealed class SendWelcomeEmailJob : IBackgroundJob<SendWelcomeEmailArgs>
{
    public Task ExecuteAsync(
        SendWelcomeEmailArgs args,
        CancellationToken cancellationToken = default
    )
    {
        // Send the email. Keep handlers retry-safe and idempotent.
        return Task.CompletedTask;
    }
}
```

Make the application module depend on `BackgroundJobModule`, then enqueue work through `IBackgroundJobManager`:

```csharp
[DependsOn(typeof(BackgroundJobModule))]
public sealed class ApplicationModule : DddModule;

await backgroundJobManager.EnqueueAsync(
    new SendWelcomeEmailArgs(userId, email),
    BackgroundJobPriority.AboveNormal,
    delay: TimeSpan.FromMinutes(1),
    cancellationToken
);
```

## Persist jobs with EF Core

Production applications can reference `modules/background-jobs/src/Ran.BackgroundJob.EntityFrameworkCore` so queued jobs survive process restarts.

```csharp
public sealed class AppDbContext(DbContextOptions<AppDbContext> options)
    : DddDbContext<AppDbContext>(options), IBackgroundJobDbContext
{
    public DbSet<BackgroundJobRecord> BackgroundJobs => Set<BackgroundJobRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ConfigureBackgroundJobs();
    }
}
```

Replace the default store in the infrastructure module, then create and apply an EF Core migration:

```csharp
[DependsOn(typeof(BackgroundJobEntityFrameworkCoreModule))]
public sealed class InfrastructureModule : DddModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddBackgroundJobEntityFrameworkCore<AppDbContext>();
    }
}
```

## Execution and retry settings

Use `BackgroundJobOptions` to control execution and JSON serialization. Use `BackgroundJobWorkerOptions` for the poll interval, batch size, initial retry delay, backoff factor, and timeout.

```csharp
Configure<BackgroundJobWorkerOptions>(options =>
{
    options.JobPollPeriod = TimeSpan.FromSeconds(10);
    options.MaxJobFetchCount = 100;
    options.DefaultFirstWaitDuration = TimeSpan.FromMinutes(1);
    options.DefaultWaitFactor = 2;
    options.DefaultTimeout = TimeSpan.FromDays(2);
});
```

Execution coordination is currently process-local. The EF Core store provides durability but does not prevent multiple application instances from fetching the same job. Before scaling out, run a single execution node or extend the store with a distributed lock or job lease.
