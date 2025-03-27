namespace Ran.Core.Modularity;

/// <summary>
/// 模块化服务配置接口
/// </summary>
public interface IModule
{
    /// <summary>
    /// 服务配置
    /// </summary>
    /// <param name="context"></param>
    void ConfigureServices(ServiceConfigurationContext context);

    /// <summary>
    /// 服务配置，异步
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    Task ConfigureServicesAsync(ServiceConfigurationContext context);
}
