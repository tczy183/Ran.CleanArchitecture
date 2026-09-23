# 后台任务

`Ran.BackgroundJob` 提供与存储实现无关的后台任务抽象，设计上参考 ABP 的类型化参数、优先级、延迟执行和指数退避重试。默认存储位于进程内，适合开发和不要求持久化的单实例应用。

## 定义并入队任务

任务处理器实现 `IBackgroundJob<TArgs>` 后会按现有约定自动注册为瞬态服务：

```csharp
public sealed record SendWelcomeEmailArgs(Guid UserId, string Email);

public sealed class SendWelcomeEmailJob : IBackgroundJob<SendWelcomeEmailArgs>
{
    public Task ExecuteAsync(
        SendWelcomeEmailArgs args,
        CancellationToken cancellationToken = default
    )
    {
        // 发送邮件。处理器应设计为可重试、可重复执行。
        return Task.CompletedTask;
    }
}
```

应用模块依赖 `BackgroundJobModule`，通过 `IBackgroundJobManager` 入队：

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

## 使用 EF Core 持久化

生产环境可引用 `modules/background-jobs/src/Ran.BackgroundJob.EntityFrameworkCore`，让任务在进程重启后仍然保留。

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

在基础设施模块中替换默认存储，然后创建并应用 EF Core migration：

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

## 执行与重试配置

通过 `BackgroundJobOptions` 控制是否执行任务和 JSON 序列化，通过 `BackgroundJobWorkerOptions` 控制轮询周期、批量大小、首次重试间隔、退避系数和超时时间。

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

当前执行协调锁是进程内的；EF Core 存储提供持久化，但不保证多个应用实例不会同时取得同一任务。多实例部署前应只启用一个执行节点，或扩展存储实现以加入分布式锁/任务租约。
