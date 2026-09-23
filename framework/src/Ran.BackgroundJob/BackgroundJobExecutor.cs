using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Ran.BackgroundJob;

public sealed class BackgroundJobExecutor(IOptions<BackgroundJobOptions> options)
    : IBackgroundJobExecutor
{
    public async Task ExecuteAsync(
        BackgroundJobInfo jobInfo,
        IServiceProvider serviceProvider,
        CancellationToken cancellationToken = default
    )
    {
        var argsType =
            Type.GetType(jobInfo.ArgsType, throwOnError: false)
            ?? throw new InvalidOperationException(
                $"Background job argument type '{jobInfo.ArgsType}' could not be loaded."
            );
        var args =
            JsonSerializer.Deserialize(jobInfo.ArgsJson, argsType, options.Value.SerializerOptions)
            ?? throw new InvalidOperationException(
                $"Arguments for background job '{jobInfo.Id}' could not be deserialized."
            );
        var handlerType = typeof(IBackgroundJob<>).MakeGenericType(argsType);
        var handler = serviceProvider.GetRequiredService(handlerType);
        var executeMethod =
            handlerType.GetMethod(nameof(IBackgroundJob<object>.ExecuteAsync))
            ?? throw new MissingMethodException(
                handlerType.FullName,
                nameof(IBackgroundJob<object>.ExecuteAsync)
            );

        try
        {
            var task =
                (Task?)executeMethod.Invoke(handler, [args, cancellationToken])
                ?? throw new InvalidOperationException(
                    $"Background job handler '{handlerType}' returned no task."
                );
            await task;
        }
        catch (TargetInvocationException exception) when (exception.InnerException is not null)
        {
            ExceptionDispatchInfo.Capture(exception.InnerException).Throw();
            throw;
        }
    }
}
