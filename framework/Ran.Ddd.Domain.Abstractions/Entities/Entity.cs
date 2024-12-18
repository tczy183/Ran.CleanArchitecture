namespace Ran.Ddd.Domain.Abstractions.Entities;

public abstract class Entity<TKey> : IEntity<TKey>
{
    protected Entity() { }

    protected Entity(TKey id)
    {
        Id = id;
    }

    public virtual object?[] GetKeys()
    {
        return [Id];
    }

    public virtual TKey Id { get; set; } = default!;
}
