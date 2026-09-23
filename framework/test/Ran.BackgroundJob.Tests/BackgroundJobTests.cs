using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace Ran.BackgroundJob.Tests;

public sealed class BackgroundJobTests
{
    private static readonly DateTimeOffset Now = new(2026, 9, 23, 0, 0, 0, TimeSpan.Zero);
    private static readonly JsonSerializerOptions SerializerOptions =
        new(JsonSerializerDefaults.Web);

    [Fact]
    public async Task Manager_should_serialize_and_schedule_job()
    {
        var store = new InMemoryBackgroundJobStore();
        var manager = new DefaultBackgroundJobManager(
            store,
            Options.Create(new BackgroundJobOptions()),
            new FixedTimeProvider(Now)
        );
        var args = new TestJobArgs("42");

        var id = await manager.EnqueueAsync(
            args,
            BackgroundJobPriority.High,
            TimeSpan.FromMinutes(5)
        );

        var jobs = await store.GetWaitingJobsAsync(10, Now.AddMinutes(5));
        var job = Assert.Single(jobs);
        Assert.Equal(id, job.Id);
        Assert.Equal(BackgroundJobPriority.High, job.Priority);
        Assert.Equal(Now.AddMinutes(5), job.NextTryTime);
        Assert.Equal(
            args,
            JsonSerializer.Deserialize<TestJobArgs>(job.ArgsJson, SerializerOptions)
        );
    }

    [Fact]
    public async Task Store_should_return_due_jobs_in_priority_order()
    {
        var store = new InMemoryBackgroundJobStore();
        await store.InsertAsync(CreateJob("normal", BackgroundJobPriority.Normal, Now));
        await store.InsertAsync(CreateJob("high", BackgroundJobPriority.High, Now));
        await store.InsertAsync(
            CreateJob("delayed", BackgroundJobPriority.High, Now.AddMinutes(1))
        );

        var jobs = await store.GetWaitingJobsAsync(10, Now);

        Assert.Collection(
            jobs,
            job => Assert.Equal("high", job.Id),
            job => Assert.Equal("normal", job.Id)
        );
    }

    [Fact]
    public async Task Executor_should_resolve_typed_handler_and_pass_arguments()
    {
        var services = new ServiceCollection();
        var handler = new TestBackgroundJob();
        services.AddSingleton<IBackgroundJob<TestJobArgs>>(handler);
        await using var provider = services.BuildServiceProvider();
        var executor = new BackgroundJobExecutor(Options.Create(new BackgroundJobOptions()));
        var args = new TestJobArgs("payload");
        var job = CreateJob("execute", BackgroundJobPriority.Normal, Now) with
        {
            ArgsType = typeof(TestJobArgs).AssemblyQualifiedName!,
            ArgsJson = JsonSerializer.Serialize(args),
        };

        await executor.ExecuteAsync(job, provider);

        Assert.Equal(args, handler.ReceivedArgs);
    }

    [Fact]
    public async Task Manager_should_reject_negative_delay()
    {
        var manager = new DefaultBackgroundJobManager(
            new InMemoryBackgroundJobStore(),
            Options.Create(new BackgroundJobOptions()),
            new FixedTimeProvider(Now)
        );

        _ = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
            () => manager.EnqueueAsync(new TestJobArgs("42"), delay: TimeSpan.FromSeconds(-1))
        );
    }

    private static BackgroundJobInfo CreateJob(
        string id,
        BackgroundJobPriority priority,
        DateTimeOffset nextTryTime
    )
    {
        return new BackgroundJobInfo
        {
            Id = id,
            JobName = typeof(TestJobArgs).FullName!,
            ArgsType = typeof(TestJobArgs).AssemblyQualifiedName!,
            ArgsJson = JsonSerializer.Serialize(new TestJobArgs(id)),
            Priority = priority,
            CreationTime = Now,
            NextTryTime = nextTryTime,
        };
    }

    public sealed record TestJobArgs(string Value);

    private sealed class TestBackgroundJob : IBackgroundJob<TestJobArgs>
    {
        public TestJobArgs? ReceivedArgs { get; private set; }

        public Task ExecuteAsync(TestJobArgs args, CancellationToken cancellationToken = default)
        {
            ReceivedArgs = args;
            return Task.CompletedTask;
        }
    }

    private sealed class FixedTimeProvider(DateTimeOffset utcNow) : TimeProvider
    {
        public override DateTimeOffset GetUtcNow() => utcNow;
    }
}
