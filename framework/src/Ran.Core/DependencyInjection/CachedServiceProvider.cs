using Ran.Core.DependencyInjection.ServiceLifetimes;

namespace Ran.Core.DependencyInjection;

/// <summary>
/// 已缓存服务提供器
/// </summary>
[ExposeServices(typeof(ICachedServiceProvider))]
public class CachedServiceProvider
    : CachedServiceProviderBase,
        ICachedServiceProvider,
        IScopedDependency
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="serviceProvider"></param>
    public CachedServiceProvider(IServiceProvider serviceProvider)
        : base(serviceProvider) { }
}
