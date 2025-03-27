using Microsoft.Extensions.Logging.Abstractions;
using Ran.Core.DependencyInjection.ServiceLifetimes;
using Ran.Core.Exceptions.Handling.Abstracts;
using Ran.Core.Extensions.Logging;
using Ran.Core.Utils.System;

namespace Ran.Core.Exceptions.Handling;

/// <summary>
/// 异常通知器
/// </summary>
public class ExceptionNotifier : IExceptionNotifier, ITransientDependency
{
    /// <summary>
    /// 日志
    /// </summary>
    public ILogger<ExceptionNotifier> Logger { get; }

    /// <summary>
    /// 服务作用域工厂
    /// </summary>
    protected IServiceScopeFactory ServiceScopeFactory { get; }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="serviceScopeFactory"></param>
    public ExceptionNotifier(IServiceScopeFactory serviceScopeFactory)
    {
        ServiceScopeFactory = serviceScopeFactory;
        Logger = NullLogger<ExceptionNotifier>.Instance;
    }

    /// <summary>
    /// 通知，异步
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public virtual async Task NotifyAsync(ExceptionNotificationContext context)
    {
        _ = CheckHelper.NotNull(context, nameof(context));

        using var scope = ServiceScopeFactory.CreateScope();
        var exceptionSubscribers = scope.ServiceProvider.GetServices<IExceptionSubscriber>();

        foreach (var exceptionSubscriber in exceptionSubscribers)
        {
            try
            {
                await exceptionSubscriber.HandleAsync(context);
            }
            catch (Exception ex)
            {
                Logger.LogWarning("抛出{AssemblyQualifiedName}类型异常。",
                    exceptionSubscriber.GetType().AssemblyQualifiedName);
                Logger.LogException(ex, LogLevel.Warning);
            }
        }
    }
}
