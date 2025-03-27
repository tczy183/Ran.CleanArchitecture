using Ran.Core.Utils.System;

namespace Ran.Core.Application;

/// <summary>
/// 应用关闭上下文
/// </summary>
public class ApplicationShutdownContext
{
    /// <summary>
    /// 服务提供者
    /// </summary>
    public IServiceProvider ServiceProvider { get; }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="serviceProvider"></param>
    public ApplicationShutdownContext(IServiceProvider serviceProvider)
    {
        _ = CheckHelper.NotNull(serviceProvider, nameof(serviceProvider));

        ServiceProvider = serviceProvider;
    }
}
