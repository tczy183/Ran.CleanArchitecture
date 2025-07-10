namespace Ran.Core.DependencyInjection;

/// <summary>
/// AutowiredServiceAttribute
/// <example>
/// 调用示例：
/// <code>
/// // 通过属性注入 Service 实例
/// public class PropertyClass
/// {
///     [AutowiredService]
///     public IService Service { get; set; }
///
///     public PropertyClass(AutowiredServiceHandler autowiredServiceHandler)
///     {
///         autowiredServiceHandler.Autowired(this);
///     }
/// }
/// // 通过字段注入 Service 实例
/// public class FieldClass
/// {
///     [AutowiredService]
///     private IService _service;
///
///     public FieldClass(AutowiredServiceHandler autowiredServiceHandler)
///     {
///         autowiredServiceHandler.Autowired(this);
///     }
/// }
/// </code>
/// </example>
/// </summary>
/// <remarks>由此启发：<see href="https://www.cnblogs.com/loogn/p/10566510.html"/></remarks>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public sealed class AutowiredServiceAttribute : Attribute;
