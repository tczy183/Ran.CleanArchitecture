using Application.Events;

using Microsoft.Extensions.Logging;

using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;

namespace Application.Handlers;

public class TestEventHandler1(ILogger<TestEventHandler1> logger)
    : ILocalEventHandler<Test1Event>, ILocalEventHandler<Test2Event>, ITransientDependency
{
    private readonly ILogger<TestEventHandler1> _logger = logger;

    public async Task HandleEventAsync(Test1Event eventData)
    {
        await Task.CompletedTask;
        _logger.LogInformation("TestEventHandler1_Test1Event: {Name}", eventData.Name);
    }

    public async Task HandleEventAsync(Test2Event eventData)
    {
        await Task.CompletedTask;
        _logger.LogInformation("TestEventHandler1_Test2Event: {Name}", eventData.Name);
    }
}

public class TestEventHandler2(ILogger<TestEventHandler2> logger) : ILocalEventHandler<Test1Event>, ILocalEventHandler<Test2Event>,ITransientDependency
{
    private readonly ILogger<TestEventHandler2> _logger = logger;

    public async Task HandleEventAsync(Test1Event eventData)
    {
        await Task.CompletedTask;
        _logger.LogInformation("TestEventHandler2_Test1Event: {Name}", eventData.Name);
    }

    public async Task HandleEventAsync(Test2Event eventData)
    {
        await Task.CompletedTask;
        _logger.LogInformation("TestEventHandler2_Test2Event: {Name}", eventData.Name);
    }
   
}