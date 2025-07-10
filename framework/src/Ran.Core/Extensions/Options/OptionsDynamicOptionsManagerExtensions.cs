using Ran.Core.Exceptions;
using Ran.Core.Options;

namespace Ran.Core.Extensions.Options;

/// <summary>
/// 配置动态选项管理器扩展方法
/// </summary>
public static class OptionsDynamicOptionsManagerExtensions
{
    /// <summary>
    /// 配置选项
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="options"></param>
    /// <returns></returns>
    public static Task SetAsync<T>(this IOptions<T> options)
        where T : class
    {
        return options.ToDynamicOptions().SetAsync();
    }

    /// <summary>
    /// 配置选项
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="options"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static Task SetAsync<T>(this IOptions<T> options, string name)
        where T : class
    {
        return options.ToDynamicOptions().SetAsync(name);
    }

    /// <summary>
    /// 转化为动态选项
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="options"></param>
    /// <returns></returns>
    /// <exception cref="UserFriendlyException"></exception>
    private static DynamicOptionsManager<T> ToDynamicOptions<T>(this IOptions<T> options)
        where T : class
    {
        return options as DynamicOptionsManager<T> ??
               throw new UserFriendlyException($"选项必须派生自 {typeof(DynamicOptionsManager<>).FullName}！");
    }
}
