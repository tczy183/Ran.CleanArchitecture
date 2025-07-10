﻿namespace Ran.Core.Options;

/// <summary>
/// 预配置泛型委托列表
/// </summary>
public class PreConfigureActionList<TOptions> : List<Action<TOptions>>
{
    /// <summary>
    /// 配置
    /// </summary>
    /// <param name="options"></param>
    public void Configure(TOptions options)
    {
        foreach (var action in this)
        {
            action(options);
        }
    }

    /// <summary>
    /// 配置
    /// </summary>
    /// <returns></returns>
    public TOptions Configure()
    {
        var options = Activator.CreateInstance<TOptions>();
        Configure(options);
        return options;
    }
}
