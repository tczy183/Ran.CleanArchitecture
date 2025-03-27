using Ran.Core.Application;
using Ran.Core.Utils.System;
using IHostEnvironment = Ran.Core.Application.IHostEnvironment;

namespace Ran.Core.Extensions.Hosting;

/// <summary>
/// 宿主环境扩展方法
/// </summary>
public static class HostEnvironmentExtensions
{
    /// <summary>
    /// 是否为开发环境
    /// </summary>
    /// <param name="hostEnvironment"></param>
    /// <returns></returns>
    public static bool IsDevelopment(this IHostEnvironment hostEnvironment)
    {
        _ = CheckHelper.NotNull(hostEnvironment, nameof(hostEnvironment));

        return hostEnvironment.IsEnvironment(Environments.Development);
    }

    /// <summary>
    /// 是否为测试环境
    /// </summary>
    /// <param name="hostEnvironment"></param>
    /// <returns></returns>
    public static bool IsStaging(this IHostEnvironment hostEnvironment)
    {
        _ = CheckHelper.NotNull(hostEnvironment, nameof(hostEnvironment));

        return hostEnvironment.IsEnvironment(Environments.Staging);
    }

    /// <summary>
    /// 是否为生产环境
    /// </summary>
    /// <param name="hostEnvironment"></param>
    /// <returns></returns>
    public static bool IsProduction(this IHostEnvironment hostEnvironment)
    {
        _ = CheckHelper.NotNull(hostEnvironment, nameof(hostEnvironment));

        return hostEnvironment.IsEnvironment(Environments.Production);
    }

    /// <summary>
    /// 是否为指定环境
    /// </summary>
    /// <param name="hostEnvironment"></param>
    /// <param name="environmentName"></param>
    /// <returns></returns>
    public static bool IsEnvironment(this IHostEnvironment hostEnvironment, string environmentName)
    {
        _ = CheckHelper.NotNull(hostEnvironment, nameof(hostEnvironment));

        return string.Equals(hostEnvironment.EnvironmentName, environmentName, StringComparison.OrdinalIgnoreCase);
    }
}
