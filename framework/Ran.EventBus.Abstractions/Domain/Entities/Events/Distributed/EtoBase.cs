namespace Ran.EventBus.Abstractions.Domain.Entities.Events.Distributed;

public abstract class EtoBase
{
    public Dictionary<string, string> Properties { get; set; } = new();
}
