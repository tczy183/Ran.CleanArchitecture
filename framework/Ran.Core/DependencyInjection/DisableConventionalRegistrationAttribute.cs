namespace Ran.Core.DependencyInjection;

/// <summary>
/// 禁止常规注册特性
/// </summary>
[AttributeUsage(AttributeTargets.All)]
public class DisableConventionalRegistrationAttribute : Attribute;
