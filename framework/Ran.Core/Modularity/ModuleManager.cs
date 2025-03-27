﻿using Ran.Core.Application;
using Ran.Core.DependencyInjection.ServiceLifetimes;
using Ran.Core.Exceptions;

namespace Ran.Core.Modularity;

/// <summary>
/// 模块管理器
/// </summary>
public class ModuleManager : IModuleManager, ISingletonDependency
{
    private readonly IModuleContainer _moduleContainer;
    private readonly IEnumerable<IModuleLifecycleContributor> _lifecycleContributors;
    private readonly ILogger<ModuleManager> _logger;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="moduleContainer"></param>
    /// <param name="logger"></param>
    /// <param name="options"></param>
    /// <param name="serviceProvider"></param>
    public ModuleManager(IModuleContainer moduleContainer, ILogger<ModuleManager> logger,
        IOptions<ModuleLifecycleOptions> options, IServiceProvider serviceProvider)
    {
        _moduleContainer = moduleContainer;
        _logger = logger;

        _lifecycleContributors = options.Value.Contributors
            .Select(serviceProvider.GetRequiredService)
            .Cast<IModuleLifecycleContributor>()
            .ToArray();
    }

    /// <summary>
    /// 初始化模块，异步
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public virtual async Task InitializeModulesAsync(ApplicationInitializationContext context)
    {
        foreach (var contributor in _lifecycleContributors)
        {
            foreach (var module in _moduleContainer.Modules)
            {
                try
                {
                    await contributor.InitializeAsync(context, module.Instance);
                }
                catch (Exception ex)
                {
                    throw new InitializationException(
                        $"在模块 {module.Type.AssemblyQualifiedName} 的初始化 {contributor.GetType().FullName} 阶段发生错误：{ex.Message}。查看集成异常以获取详细信息。",
                        ex);
                }
            }
        }
    }

    /// <summary>
    /// 初始化模块
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public void InitializeModules(ApplicationInitializationContext context)
    {
        foreach (var contributor in _lifecycleContributors)
        {
            foreach (var module in _moduleContainer.Modules)
            {
                try
                {
                    _ = contributor.InitializeAsync(context, module.Instance);
                }
                catch (Exception ex)
                {
                    throw new InitializationException(
                        $"在模块 {module.Type.AssemblyQualifiedName} 的初始化 {contributor.GetType().FullName} 阶段发生错误：{ex.Message}。查看集成异常以获取详细信息。",
                        ex);
                }
            }
        }
    }

    /// <summary>
    /// 关闭模块，异步
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public virtual async Task ShutdownModulesAsync(ApplicationShutdownContext context)
    {
        var modules = _moduleContainer.Modules.Reverse().ToList();

        foreach (var contributor in _lifecycleContributors)
        {
            foreach (var module in modules)
            {
                try
                {
                    await contributor.ShutdownAsync(context, module.Instance);
                }
                catch (Exception ex)
                {
                    throw new ShutdownException(
                        $"在模块 {module.Type.AssemblyQualifiedName} 的关闭 {contributor.GetType().FullName} 阶段发生错误：{ex.Message}。查看集成异常以获取详细信息。",
                        ex);
                }
            }
        }
    }

    /// <summary>
    /// 关闭模块
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public void ShutdownModules(ApplicationShutdownContext context)
    {
        var modules = _moduleContainer.Modules.Reverse().ToList();

        foreach (var contributor in _lifecycleContributors)
        {
            foreach (var module in modules)
            {
                try
                {
                    _ = contributor.ShutdownAsync(context, module.Instance);
                }
                catch (Exception ex)
                {
                    throw new ShutdownException(
                        $"在模块 {module.Type.AssemblyQualifiedName} 的关闭 {contributor.GetType().FullName} 阶段发生错误：{ex.Message}。查看集成异常以获取详细信息。",
                        ex);
                }
            }
        }
    }
}
