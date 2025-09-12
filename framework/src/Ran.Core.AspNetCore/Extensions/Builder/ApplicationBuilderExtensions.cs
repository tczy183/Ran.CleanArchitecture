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
    public static async Task InitializeApplicationAsync(this WebApplication app)
    {
        _ = CheckHelper.NotNull(app, nameof(app));

        app.Services.GetRequiredService<ObjectAccessor<IApplicationBuilder>>().Value =
            app;
        app.Services.GetRequiredService<ObjectAccessor<IEndpointRouteBuilder>>().Value =
            app;
        var application =
            app.Services.GetRequiredService<IApplicationWithExternalServiceProvider>();
        var applicationLifetime =
            app.Services.GetRequiredService<IHostApplicationLifetime>();

        _ = applicationLifetime.ApplicationStopping.Register(() =>
        {
            AsyncHelper.RunSync(() => application.ShutdownAsync());
        });
        _ = applicationLifetime.ApplicationStopped.Register(application.Dispose);

        await application.InitializeAsync(app.Services);
    }

    /// <summary>
    /// 初始化应用程序
    /// </summary>
    /// <param name="app"></param>
    public static void InitializeApplication(this WebApplication app)
    {
        _ = CheckHelper.NotNull(app, nameof(app));

        app.Services.GetRequiredService<ObjectAccessor<IApplicationBuilder>>().Value =
            app;
        app.Services.GetRequiredService<ObjectAccessor<IEndpointRouteBuilder>>().Value =
            app;
        var application =
            app.Services.GetRequiredService<IApplicationWithExternalServiceProvider>();
        var applicationLifetime =
            app.Services.GetRequiredService<IHostApplicationLifetime>();

        _ = applicationLifetime.ApplicationStopping.Register(application.Shutdown);
        _ = applicationLifetime.ApplicationStopped.Register(application.Dispose);

        application.Initialize(app.Services);
    }
}
