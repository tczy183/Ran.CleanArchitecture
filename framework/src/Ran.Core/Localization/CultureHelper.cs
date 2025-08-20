﻿using Ran.Core.Utils.System;
using Ran.Core.Utils.Text;
using Ran.Core.Utils.Threading;

namespace Ran.Core.Localization;

/// <summary>
/// 本地文化帮助类
/// </summary>
public static class CultureHelper
{
    /// <summary>
    /// 使用指定的文化和UI文化
    /// </summary>
    /// <param name="culture"></param>
    /// <param name="uiCulture"></param>
    /// <returns></returns>
    public static IDisposable Use([NotNull] string culture, string? uiCulture = null)
    {
        _ = CheckHelper.NotNull(culture, nameof(culture));

        return Use(new CultureInfo(culture), uiCulture is null ? null : new CultureInfo(uiCulture));
    }

    /// <summary>
    /// 使用指定的文化和UI文化
    /// </summary>
    /// <param name="culture"></param>
    /// <param name="uiCulture"></param>
    /// <returns></returns>
    public static IDisposable Use([NotNull] CultureInfo culture, CultureInfo? uiCulture = null)
    {
        _ = CheckHelper.NotNull(culture, nameof(culture));

        var currentCulture = CultureInfo.CurrentCulture;
        var currentUiCulture = CultureInfo.CurrentUICulture;

        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = uiCulture ?? culture;

        return new DisposeAction<ValueTuple<CultureInfo, CultureInfo>>(
            static (state) =>
            {
                var (currentCulture, currentUiCulture) = state;
                CultureInfo.CurrentCulture = currentCulture;
                CultureInfo.CurrentUICulture = currentUiCulture;
            },
            (currentCulture, currentUiCulture)
        );
    }

    /// <summary>
    /// 是否是文本从右向左流动的书写系统
    /// </summary>
    public static bool IsRtl => CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft;

    /// <summary>
    /// 是否为有效的文化代码
    /// </summary>
    /// <param name="cultureCode"></param>
    /// <returns></returns>
    public static bool IsValidCultureCode(string cultureCode)
    {
        if (cultureCode.IsNullOrWhiteSpace())
        {
            return false;
        }

        try
        {
            _ = CultureInfo.GetCultureInfo(cultureCode);
            return true;
        }
        catch (CultureNotFoundException)
        {
            return false;
        }
    }

    /// <summary>
    /// 获取基础文化名称
    /// </summary>
    /// <param name="cultureName"></param>
    /// <returns></returns>
    public static string GetBaseCultureName(string cultureName)
    {
        return new CultureInfo(cultureName).Parent.Name;
    }

    /// <summary>
    /// 是否为兼容的文化
    /// </summary>
    /// <param name="sourceCultureName"></param>
    /// <param name="targetCultureName"></param>
    /// <returns></returns>
    public static bool IsCompatibleCulture(string sourceCultureName, string targetCultureName)
    {
        if (sourceCultureName == targetCultureName)
        {
            return true;
        }

        if (sourceCultureName.StartsWith("zh") && targetCultureName.StartsWith("zh"))
        {
            var culture = new CultureInfo(targetCultureName);
            do
            {
                if (culture.Name == sourceCultureName)
                {
                    return true;
                }

                culture = new CultureInfo(culture.Name).Parent;
            } while (!culture.Equals(CultureInfo.InvariantCulture));
        }

        return !sourceCultureName.Contains('-')
            && targetCultureName.Contains('-')
            && sourceCultureName == GetBaseCultureName(targetCultureName);
    }
}
