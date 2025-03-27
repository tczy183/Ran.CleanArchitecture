using Ran.Core.Logging;

namespace Ran.Core.Extensions.DependencyInjection;

/// <summary>
/// 获取初始化日志
/// </summary>
public static class ServiceCollectionLoggingExtensions
{
    /// <summary>
    /// 获取初始化日志工厂
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="services"></param>
    /// <returns></returns>
    public static ILogger<T> GetInitLogger<T>(this IServiceCollection services)
    {
        return services.GetSingletonInstance<IInitLoggerFactory>().Create<T>();
    }
}
