﻿using Ran.Core.Utils.DataFilter.Paging.Enums;

namespace Ran.Core.Utils.DataFilter.Paging.Dtos;

/// <summary>
/// 通用选择条件基类
/// </summary>
[Serializable]
public class SelectConditionDto
{
    /// <summary>
    /// 构造一个选择字段名称和选择值的选择条件
    /// </summary>
    /// <param name="selectField">字段名称</param>
    /// <param name="criteriaValue">条件值</param>
    public SelectConditionDto(string selectField, object? criteriaValue)
    {
        SelectField = selectField;
        CriteriaValue = criteriaValue;
    }

    /// <summary>
    /// 构造一个选择字段名称和选择值的选择条件
    /// </summary>
    /// <param name="selectField">字段名称</param>
    /// <param name="criteriaValue">条件值</param>
    /// <param name="selectCompare">选择比较</param>
    public SelectConditionDto(string selectField, object? criteriaValue, SelectCompareEnum selectCompare)
    {
        SelectField = selectField;
        CriteriaValue = criteriaValue;
        SelectCompare = selectCompare;
    }

    /// <summary>
    /// 构造一个选择字段名称和选择值的选择条件
    /// </summary>
    /// <param name="isKeywords">是否关键字</param>
    /// <param name="selectField">字段名称</param>
    /// <param name="criteriaValue">条件值</param>
    /// <param name="selectCompare">选择比较</param>
    public SelectConditionDto(bool isKeywords, string selectField, object? criteriaValue,
        SelectCompareEnum selectCompare)
    {
        IsKeywords = isKeywords;
        SelectField = selectField;
        CriteriaValue = criteriaValue;
        SelectCompare = selectCompare;
    }

    /// <summary>
    /// 是否关键字
    /// </summary>
    public bool IsKeywords { get; set; }

    /// <summary>
    /// 选择字段
    /// </summary>
    public string SelectField { get; set; }

    /// <summary>
    /// 条件值
    /// </summary>
    public object? CriteriaValue { get; set; }

    /// <summary>
    /// 选择比较，默认为等于
    /// </summary>
    public SelectCompareEnum SelectCompare { get; set; } = SelectCompareEnum.Equal;
}

/// <summary>
/// 通用选择条件泛型基类
/// </summary>
/// <typeparam name="T">列表元素类型</typeparam>
[Serializable]
public class SelectConditionDto<T> : SelectConditionDto
{
    /// <summary>
    /// 使用选择字段名称和选择值，初始化一个<see cref="SelectConditionDto"/>类型的新实例
    /// </summary>
    /// <param name="selectField">字段名称</param>
    /// <param name="criteriaValue">条件值</param>
    public SelectConditionDto(string selectField, object? criteriaValue)
        : base(selectField, criteriaValue!)
    {
    }

    /// <summary>
    /// 使用选择字段名称和选择值，初始化一个<see cref="SelectConditionDto"/>类型的新实例
    /// </summary>
    /// <param name="selectField">字段名称</param>
    /// <param name="criteriaValue">条件值</param>
    /// <param name="selectCompare">选择比较</param>
    public SelectConditionDto(string selectField, object? criteriaValue, SelectCompareEnum selectCompare)
        : base(selectField, criteriaValue!, selectCompare)
    {
    }

    /// <summary>
    /// 使用选择字段名称和选择值，初始化一个<see cref="SelectConditionDto"/>类型的新实例
    /// </summary>
    /// <param name="isKeywords">是否关键字</param>
    /// <param name="selectField">字段名称</param>
    /// <param name="criteriaValue">条件值</param>
    /// <param name="selectCompare">选择比较</param>
    public SelectConditionDto(bool isKeywords, string selectField, object? criteriaValue,
        SelectCompareEnum selectCompare)
        : base(isKeywords, selectField, criteriaValue!, selectCompare)
    {
    }
}
