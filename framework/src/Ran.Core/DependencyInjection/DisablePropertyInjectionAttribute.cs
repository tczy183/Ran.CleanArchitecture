namespace Ran.Core.DependencyInjection;

/// <summary>
/// 禁用属性注入特性
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
public sealed class DisablePropertyInjectionAttribute : Attribute;
