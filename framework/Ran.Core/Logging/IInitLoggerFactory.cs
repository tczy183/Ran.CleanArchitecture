namespace Ran.Core.Logging;

/// <summary>
/// 初始化日志工厂接口
/// </summary>
public interface IInitLoggerFactory
{
    /// <summary>
    /// 创建初始化日志
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    IInitLogger<T> Create<T>();
}
