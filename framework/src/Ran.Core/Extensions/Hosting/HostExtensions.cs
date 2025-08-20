using Ran.Core.Application;
using Ran.Core.Utils.Threading;

namespace Ran.Core.Extensions.Hosting;

/// <summary>
/// 主机扩展方法
/// </summary>
public static class HostExtensions
{
    /// <summary>
    /// 异步初始化应用程序
    /// </summary>
    /// <param name="host"></param>
    /// <returns></returns>
    public static async Task InitializeAsync(this IHost host)
    {
        var application =
            host.Services.GetRequiredService<IApplicationWithExternalServiceProvider>();
        var applicationLifetime = host.Services.GetRequiredService<IHostApplicationLifetime>();

        _ = applicationLifetime.ApplicationStopping.Register(() =>
            AsyncHelper.RunSync(() => application.ShutdownAsync())
        );
        _ = applicationLifetime.ApplicationStopped.Register(application.Dispose);

        await application.InitializeAsync(host.Services);
    }
}
