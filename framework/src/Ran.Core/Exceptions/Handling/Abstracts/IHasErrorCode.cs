namespace Ran.Core.Exceptions.Handling.Abstracts;

/// <summary>
/// 异常代码接口
/// </summary>
public interface IHasErrorCode
{
    /// <summary>
    /// 异常代码
    /// </summary>
    string? Code { get; }
}
