namespace Ran.Core.DependencyInjection;

/// <summary>
/// 客户端作用域服务提供访问器
/// </summary>
public interface IClientScopeServiceProviderAccessor
{
    /// <summary>
    /// 服务提供器
    /// </summary>
    IServiceProvider ServiceProvider { get; }
}
