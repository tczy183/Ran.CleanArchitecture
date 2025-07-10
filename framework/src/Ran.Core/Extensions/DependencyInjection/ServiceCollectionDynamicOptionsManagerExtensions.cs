using Ran.Core.Options;

namespace Ran.Core.Extensions.DependencyInjection;

/// <summary>
/// 服务集合动态选项管理器扩展方法
/// </summary>
public static class ServiceCollectionDynamicOptionsManagerExtensions
{
    /// <summary>
    /// 添加动态选项
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    /// <typeparam name="TManager"></typeparam>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddXiHanDynamicOptions<TOptions, TManager>(this IServiceCollection services)
        where TOptions : class
        where TManager : DynamicOptionsManager<TOptions>
    {
        _ = services.Replace(ServiceDescriptor.Scoped<IOptions<TOptions>, TManager>());
        _ = services.Replace(ServiceDescriptor.Scoped<IOptionsSnapshot<TOptions>, TManager>());

        return services;
    }
}
