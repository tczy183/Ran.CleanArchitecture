namespace Ran.EventBus.Abstractions.EventBus.Local;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public sealed class LocalEventHandlerOrderAttribute(int order) : Attribute
{
    /// <summary>
    /// Handlers execute in ascending numeric value of the Order property.
    /// </summary>
    public int Order { get; set; } = order;
}
