using Ran.Core.Utils.System;

namespace Ran.Core.Extensions.DependencyInjection;

/// <summary>
/// 服务提供者键控服务扩展方法
/// </summary>
public static class ServiceProviderKeyedServiceExtensions
{
    /// <summary>
    /// 获取键控服务
    /// </summary>
    /// <param name="provider"></param>
    /// <param name="serviceType"></param>
    /// <param name="serviceKey"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static object? GetKeyedService(this IServiceProvider provider, Type serviceType, object? serviceKey)
    {
        _ = CheckHelper.NotNull(provider, nameof(provider));

        return provider is IKeyedServiceProvider keyedServiceProvider
            ? keyedServiceProvider.GetKeyedService(serviceType, serviceKey)
            : throw new InvalidOperationException("这个服务提供者不支持键控服务。 ");
    }
}
