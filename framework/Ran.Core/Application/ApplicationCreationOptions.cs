using Ran.Core.Extensions.Configuration;
using Ran.Core.Modularity.PlugIns;
using Ran.Core.Utils.System;

namespace Ran.Core.Application;

/// <summary>
/// 应用创建选项
/// </summary>
public class ApplicationCreationOptions
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="services"></param>
    public ApplicationCreationOptions(IServiceCollection services)
    {
        Services = CheckHelper.NotNull(services, nameof(services));
        PlugInSources = [];
        Configuration = new ConfigurationBuilderOptions();
    }

    /// <summary>
    /// 服务容器
    /// </summary>
    public IServiceCollection Services { get; }

    /// <summary>
    /// 插件源列表
    /// </summary>
    public PlugInSourceList PlugInSources { get; }

    /// <summary>
    /// 此属性中的选项仅在未注册 IConfiguration 时生效
    /// </summary>
    public ConfigurationBuilderOptions Configuration { get; }

    /// <summary>
    /// 是否跳过配置服务
    /// </summary>
    public bool SkipConfigureServices { get; set; }

    /// <summary>
    /// 应用名称
    /// </summary>
    public string? ApplicationName { get; set; }

    /// <summary>
    /// 环境
    /// </summary>
    public string? Environment { get; set; }
}
