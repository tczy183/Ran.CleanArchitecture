using Ran.Core.Application;
using Ran.Core.Utils.System;
using Ran.Core.Utils.Threading;

namespace Ran.Core.AspNetCore.Extensions.Builder;

public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// 初始化应用程序
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static async Task InitializeApplicationAsync(this IApplicationBuilder app)
    {
        _ = CheckHelper.NotNull(app, nameof(app));

        app.ApplicationServices.GetRequiredService<ObjectAccessor<IApplicationBuilder>>().Value = app;
        var application = app.ApplicationServices.GetRequiredService<IApplicationWithExternalServiceProvider>();
        var applicationLifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();

        _ = applicationLifetime.ApplicationStopping.Register(() =>
        {
            AsyncHelper.RunSync(() => application.ShutdownAsync());
        });
        _ = applicationLifetime.ApplicationStopped.Register(application.Dispose);

        await application.InitializeAsync(app.ApplicationServices);
    }

    /// <summary>
    /// 初始化应用程序
    /// </summary>
    /// <param name="app"></param>
    public static void InitializeApplication(this IApplicationBuilder app)
    {
        _ = CheckHelper.NotNull(app, nameof(app));

        app.ApplicationServices.GetRequiredService<ObjectAccessor<IApplicationBuilder>>().Value = app;
        var application = app.ApplicationServices.GetRequiredService<IApplicationWithExternalServiceProvider>();
        var applicationLifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();

        _ = applicationLifetime.ApplicationStopping.Register(application.Shutdown);
        _ = applicationLifetime.ApplicationStopped.Register(application.Dispose);

        application.Initialize(app.ApplicationServices);
    }
}
