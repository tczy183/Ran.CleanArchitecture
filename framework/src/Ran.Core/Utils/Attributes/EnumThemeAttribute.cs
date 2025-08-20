namespace Ran.Core.Utils.Attributes;

/// <summary>
/// 枚举主题特性
/// </summary>
[AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field)]
public sealed class EnumThemeAttribute : Attribute
{
    /// <summary>
    /// 主题
    /// </summary>
    public string Theme { get; private set; }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="type"></param>
    public EnumThemeAttribute(ThemeType type)
    {
        Theme = type.ToString().ToLower();
    }
}

/// <summary>
/// 主题类型 default、tertiary、primary、info、success、warning 和 error
/// </summary>
public enum ThemeType
{
    /// <summary>
    /// 默认
    /// </summary>
    [Description("default")]
    Default,

    /// <summary>
    /// 三级
    /// </summary>
    [Description("tertiary")]
    Tertiary,

    /// <summary>
    /// 主要
    /// </summary>
    [Description("primary")]
    Primary,

    /// <summary>
    /// 信息
    /// </summary>
    [Description("info")]
    Info,

    /// <summary>
    /// 成功
    /// </summary>
    [Description("success")]
    Success,

    /// <summary>
    /// 警告
    /// </summary>
    [Description("warning")]
    Warning,

    /// <summary>
    /// 错误
    /// </summary>
    [Description("error")]
    Error,
}
