namespace Ran.EventBus.Abstractions.EventBus;

public interface IEventHandlerDisposeWrapper : IDisposable
{
    IEventHandler EventHandler { get; }
}
