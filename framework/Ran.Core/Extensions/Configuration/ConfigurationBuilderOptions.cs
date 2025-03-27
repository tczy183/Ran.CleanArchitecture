namespace Ran.Core.Extensions.Configuration;

/// <summary>
/// 配置生成器选项
/// </summary>
public class ConfigurationBuilderOptions
{
    /// <summary>
    /// 用于设置获取应用程序用户密钥 ID 的程序集
    /// 可以使用此属性或 <see cref="UserSecretsId"/>（优先级更高）
    /// </summary>
    public Assembly? UserSecretsAssembly { get; set; }

    /// <summary>
    /// 用于设置应用程序的用户密钥 ID
    /// 可以使用此属性或<see cref="UserSecretsAssembly"/>（优先级更高）
    /// </summary>
    public string? UserSecretsId { get; set; }

    /// <summary>
    /// 配置文件的名称，默认值为"appsettings"
    /// </summary>
    public string FileName { get; } = "appsettings";

    /// <summary>
    /// 配置文件是否可配置，默认值为 true
    /// </summary>
    public bool Optional { get; } = true;

    /// <summary>
    /// 当文件发生更改时，是否应重新加载配置，默认值为 true
    /// </summary>
    public bool ReloadOnChange { get; } = true;

    /// <summary>
    /// 环境名称，通常使用"Development"、"Staging"或"Production"
    /// </summary>
    public string? EnvironmentName { get; set; }

    /// <summary>
    /// 读取由 <see cref="FileName"/> 指示的配置文件的基本路径
    /// </summary>
    public string? BasePath { get; set; }

    /// <summary>
    /// 环境变量的前缀
    /// </summary>
    public string? EnvironmentVariablesPrefix { get; set; }

    /// <summary>
    /// 命令行参数
    /// </summary>
    public string[]? CommandLineArgs { get; set; }
}
