using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Ran.Core.Extensions.DependencyInjection;
using Ran.Core.Modularity;

namespace Ran.Core.AspNetCore;

public class AspNetCoreModule : DddModule
{
    /// <summary>
    /// 服务配置前
    /// </summary>
    /// <param name="context"></param>
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        var services = context.Services;

        services.GetSingletonInstance<IHostEnvironment>();
    }

    /// <summary>
    /// 服务配置
    /// </summary>
    /// <param name="context"></param>
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var services = context.Services;

        _ = services.AddObjectAccessor<IApplicationBuilder>();
    }
}
