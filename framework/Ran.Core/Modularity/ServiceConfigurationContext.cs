﻿using Ran.Core.Utils.Collections;
using Ran.Core.Utils.System;

namespace Ran.Core.Modularity;

/// <summary>
/// 服务配置上下文
/// </summary>
public class ServiceConfigurationContext
{
    /// <summary>
    /// 服务存储器
    /// </summary>
    public IDictionary<string, object?> Items { get; }

    /// <summary>
    /// 服务
    /// </summary>
    public IServiceCollection Services { get; }

    /// <summary>
    /// 过程中可以存储的任意命名对象，服务注册阶段并在模块之间共享
    /// 这是<see cref="Items"/> 字典的一种快捷用法
    /// 如果给定的键在<see cref="Items"/> 字典中没有找到，则返回 null
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public object? this[string key]
    {
        get => Items.GetOrDefault(key);
        set => Items[key] = value;
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="services"></param>
    public ServiceConfigurationContext(IServiceCollection services)
    {
        Services = CheckHelper.NotNull(services, nameof(services));
        Items = new Dictionary<string, object?>();
    }
}
