using Ran.Core.Utils.Collections;

namespace Ran.Core.Logging;

/// <summary>
/// 默认初始化日志工厂
/// </summary>
public sealed class DefaultInitLoggerFactory : IInitLoggerFactory
{
    private readonly Dictionary<Type, object> _cache = [];

    /// <summary>
    /// 创建初始化日志
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public IInitLogger<T> Create<T>()
    {
        return (IInitLogger<T>)_cache.GetOrAdd(typeof(T), () => new DefaultInitLogger<T>());
    }
}
