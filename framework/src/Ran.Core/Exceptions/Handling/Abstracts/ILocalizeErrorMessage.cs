using Ran.Core.Localization;

namespace Ran.Core.Exceptions.Handling.Abstracts;

/// <summary>
/// 本地化报错信息接口
/// </summary>
public interface ILocalizeErrorMessage
{
    /// <summary>
    /// 本地化报错信息
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    string LocalizeErrorMessage(LocalizationContext context);
}
