namespace Ran.Core.Exceptions.Handling.Abstracts;

/// <summary>
/// 网络请求状态码接口
/// </summary>
public interface IHasHttpStatusCode
{
    /// <summary>
    /// 网络请求状态码
    /// </summary>
    int HttpStatusCode { get; }
}
