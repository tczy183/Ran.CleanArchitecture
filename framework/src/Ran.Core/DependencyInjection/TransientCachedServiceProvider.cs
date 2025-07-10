using Ran.Core.DependencyInjection.ServiceLifetimes;

namespace Ran.Core.DependencyInjection;

/// <summary>
/// 瞬时缓存服务提供器
/// </summary>
[ExposeServices(typeof(ITransientCachedServiceProvider))]
public class TransientCachedServiceProvider : CachedServiceProviderBase, ITransientCachedServiceProvider,
    ITransientDependency
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="serviceProvider"></param>
    public TransientCachedServiceProvider(IServiceProvider serviceProvider)
        : base(serviceProvider)
    {
    }
}
