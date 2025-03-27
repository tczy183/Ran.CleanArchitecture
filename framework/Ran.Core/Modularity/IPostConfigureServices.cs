namespace Ran.Core.Modularity;

/// <summary>
/// 服务配置后接口
/// </summary>
public interface IPostConfigureServices
{
    /// <summary>
    /// 服务配置后，异步
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    Task PostConfigureServicesAsync(ServiceConfigurationContext context);

    /// <summary>
    /// 服务配置后
    /// </summary>
    /// <param name="context"></param>
    void PostConfigureServices(ServiceConfigurationContext context);
}
