namespace Ran.Core.DependencyInjection;

/// <summary>
/// 服务提供者访问器接口
/// </summary>
public interface IServiceProviderAccessor
{
    /// <summary>
    /// 服务提供者
    /// </summary>
    IServiceProvider ServiceProvider { get; }
}
