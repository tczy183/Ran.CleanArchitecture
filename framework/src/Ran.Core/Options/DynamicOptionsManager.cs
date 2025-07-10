namespace Ran.Core.Options;

/// <summary>
/// 动态选项管理器
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class DynamicOptionsManager<T> : OptionsManager<T>
    where T : class
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="factory"></param>
    protected DynamicOptionsManager(IOptionsFactory<T> factory)
        : base(factory)
    {
    }

    /// <summary>
    /// 设置
    /// </summary>
    /// <returns></returns>
    public async Task SetAsync()
    {
        await SetAsync(string.Empty);
    }

    /// <summary>
    /// 设置
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public virtual Task SetAsync(string name)
    {
        return OverrideOptionsAsync(name, base.Get(name));
    }

    /// <summary>
    /// 重写选项
    /// </summary>
    /// <param name="name"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    protected abstract Task OverrideOptionsAsync(string name, T options);
}
