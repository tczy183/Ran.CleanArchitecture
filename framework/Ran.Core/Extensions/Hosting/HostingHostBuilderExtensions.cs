namespace Ran.Core.Extensions.Hosting;

/// <summary>
/// 主机构建器扩展方法
/// </summary>
public static class HostingHostBuilderExtensions
{
    /// <summary>
    /// 应用私密信息设置 JSON 路径
    /// </summary>
    public const string AppSettingsSecretJsonPath = "appsettings.secrets.json";

    /// <summary>
    /// 添加应用设置的私密 JSON
    /// </summary>
    /// <param name="hostBuilder"></param>
    /// <param name="optional"></param>
    /// <param name="reloadOnChange"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    public static IHostBuilder AddAppSettingsSecretsJson(
        this IHostBuilder hostBuilder,
        bool optional = true,
        bool reloadOnChange = true,
        string path = AppSettingsSecretJsonPath)
    {
        return hostBuilder.ConfigureAppConfiguration((_, builder) =>
        {
            _ = (HostBuilderContext)builder.AddJsonFile(path, optional, reloadOnChange);
        });
    }
}
