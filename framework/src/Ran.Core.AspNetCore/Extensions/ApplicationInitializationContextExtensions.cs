using Ran.Core.Application;
using Ran.Core.Utils.System;

namespace Ran.Core.AspNetCore.Extensions;

public static class ApplicationInitializationContextExtensions
{
    /// <summary>
    /// 获取应用程序构建器
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static IApplicationBuilder GetApplicationBuilder(
        this ApplicationInitializationContext context
    )
    {
        var applicationBuilder = context
            .ServiceProvider.GetRequiredService<IObjectAccessor<IApplicationBuilder>>()
            .Value;

        _ = CheckHelper.NotNull(applicationBuilder, nameof(applicationBuilder));

        return applicationBuilder!;
    }

    /// <summary>
    /// 获取应用程序构建器
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static IApplicationBuilder? GetApplicationBuilderOrNull(
        this ApplicationInitializationContext context
    )
    {
        return context
            .ServiceProvider.GetRequiredService<IObjectAccessor<IApplicationBuilder>>()
            .Value;
    }

    /// <summary>
    /// 获取
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static IEndpointRouteBuilder GetEndpointRouteBuilder(
        this ApplicationInitializationContext context
    )
    {
        var endpointRouteBuilder = context
            .ServiceProvider.GetRequiredService<IObjectAccessor<IEndpointRouteBuilder>>()
            .Value;

        _ = CheckHelper.NotNull(endpointRouteBuilder, nameof(endpointRouteBuilder));

        return endpointRouteBuilder!;
    }

    /// <summary>
    /// 获取
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static IEndpointRouteBuilder GetEndpointRouteBuilderOrNull(
        this ApplicationInitializationContext context
    )
    {
        return context
            .ServiceProvider.GetRequiredService<IObjectAccessor<IEndpointRouteBuilder>>()
            .Value;
    }

    /// <summary>
    /// 获取环境
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static IWebHostEnvironment GetEnvironment(this ApplicationInitializationContext context)
    {
        return context.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
    }

    /// <summary>
    /// 获取环境
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static IWebHostEnvironment? GetEnvironmentOrNull(
        this ApplicationInitializationContext context
    )
    {
        return context.ServiceProvider.GetService<IWebHostEnvironment>();
    }

    /// <summary>
    /// 获取配置
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static IConfiguration GetConfiguration(this ApplicationInitializationContext context)
    {
        return context.ServiceProvider.GetRequiredService<IConfiguration>();
    }

    /// <summary>
    /// 获取日志工厂
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static ILoggerFactory GetLoggerFactory(this ApplicationInitializationContext context)
    {
        return context.ServiceProvider.GetRequiredService<ILoggerFactory>();
    }
}
