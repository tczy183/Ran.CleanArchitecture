namespace Ran.Core.Exceptions.Handling.Abstracts;

/// <summary>
/// 异常详情接口
/// </summary>
public interface IHasErrorDetails
{
    /// <summary>
    /// 异常详情
    /// </summary>
    string? Details { get; }
}
