using System.Collections.ObjectModel;

namespace Ran.Ddd.Domain.Abstractions.Entities;

public abstract class AggregateRoot<TKey> : Entity<TKey>, IAggregateRoot<TKey>
{
    protected AggregateRoot()
    {
        ConcurrencyStamp = Guid.NewGuid().ToString();
    }

    protected AggregateRoot(TKey id)
        : base(id)
    {
        ConcurrencyStamp = Guid.NewGuid().ToString();
    }

    public string ConcurrencyStamp { get; set; }
    private readonly ICollection<DomainEventRecord> _distributedEvents =
        new Collection<DomainEventRecord>();
    private readonly ICollection<DomainEventRecord> _localEvents =
        new Collection<DomainEventRecord>();

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
        _distributedEvents.Clear();
    }

    public void ClearDistributedEvents()
    {
        throw new NotImplementedException();
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
