using System.Collections.ObjectModel;

namespace Ran.Ddd.Domain.Abstraction.Entities;

public abstract class AggregateRoot<TKey> : Entity<TKey>, IAggregateRoot<TKey>
{
    public string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();

    private readonly ICollection<DomainEventRecord> _distributedEvents =
        [];

    private readonly ICollection<DomainEventRecord> _localEvents =
        [];

    public IEnumerable<DomainEventRecord> GetLocalEvents()
    {
        return _localEvents;
    }

    public IEnumerable<DomainEventRecord> GetDistributedEvents()
    {
        return _distributedEvents;
    }

    public void ClearLocalEvents()
    {
        _localEvents.Clear();
    }

    public void ClearDistributedEvents()
    {
        _distributedEvents.Clear();
    }

    protected virtual void AddLocalEvent(object eventData)
    {
        _localEvents.Add(new DomainEventRecord(eventData, EventOrderGenerator.GetNext()));
    }

    protected virtual void AddDistributedEvent(object eventData)
    {
        _distributedEvents.Add(new DomainEventRecord(eventData, EventOrderGenerator.GetNext()));
    }
}
