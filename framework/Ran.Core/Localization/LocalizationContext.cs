using Microsoft.Extensions.Localization;
using Ran.Core.DependencyInjection;

namespace Ran.Core.Localization;

/// <summary>
/// 本地化上下文
/// </summary>
public class LocalizationContext : IServiceProviderAccessor
{
    /// <summary>
    /// 服务提供者
    /// </summary>
    public IServiceProvider ServiceProvider { get; }

    /// <summary>
    /// 本地化工厂
    /// </summary>
    public IStringLocalizerFactory LocalizerFactory { get; }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="serviceProvider"></param>
    public LocalizationContext(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
        LocalizerFactory = ServiceProvider.GetRequiredService<IStringLocalizerFactory>();
    }
}
