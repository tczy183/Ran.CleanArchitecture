namespace Ran.Core.Application;

/// <summary>
/// 具有集成服务的应用提供器接口
/// </summary>
public interface IApplicationWithInternalServiceProvider : IApplication
{
    /// <summary>
    /// 创建服务提供器，但不初始化模块。
    /// 多次调用将返回相同的服务提供器，而不会再次创建
    /// </summary>
    IServiceProvider CreateServiceProvider();

    /// <summary>
    /// 创建服务提供商并初始化所有模块，异步
    /// 如果之前调用过 <see cref="CreateServiceProvider"/> 方法，它不会重新创建，而是使用之前的那个
    /// </summary>
    Task InitializeAsync();

    /// <summary>
    /// 创建服务提供商并初始化所有模块
    /// 如果之前调用过 <see cref="CreateServiceProvider"/> 方法，它不会重新创建，而是使用之前的那个
    /// </summary>
    void Initialize();
}
