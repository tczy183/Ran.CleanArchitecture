using System.ComponentModel.DataAnnotations;
using Ran.Core.Utils.Attributes;

namespace Ran.Core.Utils.Reflections;

/// <summary>
/// 字段信息扩展方法
/// </summary>
public static class FieldInfoExtensions
{
    /// <summary>
    /// 获取字段描述特性的值
    /// </summary>
    /// <param name="field"></param>
    /// <returns></returns>
    public static string GetDescriptionValue(this FieldInfo field)
    {
        var descValue = field.Name;
        if (field.GetCustomAttribute(typeof(DescriptionAttribute), false) is DescriptionAttribute description)
        {
            descValue = !string.IsNullOrEmpty(description.Description) ? description.Description : descValue;
        }

        return descValue;
    }

    /// <summary>
    /// 获取字段描述特性的值
    /// </summary>
    /// <param name="field"></param>
    /// <returns></returns>
    public static string GetDisplayValue(this FieldInfo field)
    {
        var displayValue = field.Name;
        if (field.GetCustomAttribute(typeof(DisplayAttribute), false) is DisplayAttribute display)
        {
            displayValue = !string.IsNullOrEmpty(display.Description) ? display.Description : displayValue;
        }

        return displayValue;
    }

    /// <summary>
    /// 获取字段主题特性的值
    /// </summary>
    /// <param name="field"></param>
    /// <returns></returns>
    public static string GetThemeValue(this FieldInfo field)
    {
        var themeValue = ThemeType.Default.ToString().ToLower();
        if (field.GetCustomAttribute<EnumThemeAttribute>(false) is EnumThemeAttribute theme)
        {
            themeValue = theme.Theme;
        }

        return themeValue;
    }
}
