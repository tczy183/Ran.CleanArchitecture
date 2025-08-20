﻿using System.ComponentModel.DataAnnotations;

namespace Ran.Core.Utils.Reflections;

/// <summary>
/// 成员信息扩展方法
/// </summary>
public static class MemberInfoExtensions
{
    #region 描述信息

    /// <summary>
    /// 获取成员元数据的 Description 特性描述信息
    /// </summary>
    /// <param name="member">成员元数据对象</param>
    /// <param name="inherit">是否搜索成员的继承链以查找描述特性</param>
    /// <returns>返回 Description 特性描述信息，如不存在则返回成员的名称</returns>
    public static string GetDescription(this MemberInfo member, bool inherit = true)
    {
        var desc = member.GetSingleAttributeOrNull<DescriptionAttribute>(inherit);
        if (desc is not null)
        {
            return desc.Description;
        }

        var displayName = member.GetSingleAttributeOrNull<DisplayNameAttribute>(inherit);
        if (displayName is not null)
        {
            return displayName.DisplayName;
        }

        var display = member.GetSingleAttributeOrNull<DisplayAttribute>(inherit);
        return display is not null ? display.Name ?? string.Empty : member.Name;
    }

    #endregion 描述信息

    #region 特性信息

    /// <summary>
    /// 检查指定指定类型成员中是否存在指定的 Attribute 特性
    /// </summary>
    /// <typeparam name="TAttribute">要检查的 Attribute 特性类型</typeparam>
    /// <param name="memberInfo">要检查的成员</param>
    /// <param name="inherit">是否从继承中查找</param>
    /// <returns>是否存在</returns>
    public static bool HasAttribute<TAttribute>(this MemberInfo memberInfo, bool inherit = true)
        where TAttribute : Attribute
    {
        return memberInfo.IsDefined(typeof(TAttribute), inherit);
    }

    /// <summary>
    /// 获取成员的单个特性
    /// </summary>
    /// <typeparam name="TAttribute">要检查的 Attribute 特性类型</typeparam>
    /// <param name="memberInfo">要检查的成员</param>
    /// <param name="inherit">是否从继承中查找</param>
    /// <returns>Returns the attribute object if found. Returns null if not found.</returns>
    public static TAttribute? GetSingleAttributeOrNull<TAttribute>(
        this MemberInfo memberInfo,
        bool inherit = true
    )
        where TAttribute : Attribute
    {
        ArgumentNullException.ThrowIfNull(memberInfo);

        var attrs = memberInfo.GetCustomAttributes(typeof(TAttribute), inherit).ToArray();
        return attrs.Length > 0 ? (TAttribute)attrs[0] : default;
    }

    /// <summary>
    /// 获取特定类型或基类型的单个属性（或为 null）
    /// </summary>
    /// <typeparam name="TAttribute">要检查的 Attribute 特性类型</typeparam>
    /// <param name="type">要检查的类型成员</param>
    /// <returns></returns>
    public static TAttribute? GetSingleAttributeOfTypeOrBaseTypesOrNull<TAttribute>(this Type? type)
        where TAttribute : Attribute
    {
        while (true)
        {
            var attr = type?.GetTypeInfo().GetSingleAttributeOrNull<TAttribute>();
            if (attr is not null)
            {
                return attr;
            }

            if (type is not null && type.GetTypeInfo().BaseType is null)
            {
                return null;
            }

            type = type?.GetTypeInfo().BaseType;
        }
    }

    #endregion 特性信息
}
