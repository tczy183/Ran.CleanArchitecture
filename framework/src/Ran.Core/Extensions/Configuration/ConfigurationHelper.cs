using Ran.Core.Utils.Text;

namespace Ran.Core.Extensions.Configuration;

/// <summary>
/// 配置帮助类
/// </summary>
public static class ConfigurationHelper
{
    /// <summary>
    /// 绑定配置
    /// </summary>
    /// <param name="options"></param>
    /// <param name="builderAction"></param>
    /// <returns></returns>
    public static IConfigurationRoot BuildConfiguration(ConfigurationBuilderOptions? options = null,
        Action<IConfigurationBuilder>? builderAction = null)
    {
        options ??= new ConfigurationBuilderOptions();

        // 设置基础路径
        if (options.BasePath.IsNullOrEmpty())
        {
            options.BasePath = Directory.GetCurrentDirectory();
        }

        // 加载基础配置文件
        var builder = new ConfigurationBuilder()
            .SetBasePath(options.BasePath!)
            .AddJsonFile(options.FileName + ".json", options.Optional, options.ReloadOnChange)
            .AddJsonFile(options.FileName + ".secrets.json", true, options.ReloadOnChange);

        // 加载特定环境下的配置文件
        if (!options.EnvironmentName.IsNullOrEmpty())
        {
            builder = builder.AddJsonFile($"{options.FileName}.{options.EnvironmentName}.json", true,
                options.ReloadOnChange);
        }

        // 开发环境，加载用户机密
        if (options.EnvironmentName == "Development")
        {
            if (options.UserSecretsId is not null)
            {
                _ = builder.AddUserSecrets(options.UserSecretsId);
            }
            else if (options.UserSecretsAssembly is not null)
            {
                _ = builder.AddUserSecrets(options.UserSecretsAssembly, true);
            }
        }

        builder = builder.AddEnvironmentVariables(options.EnvironmentVariablesPrefix);

        if (options.CommandLineArgs is not null)
        {
            builder = builder.AddCommandLine(options.CommandLineArgs);
        }

        builderAction?.Invoke(builder);

        return builder.Build();
    }
}
