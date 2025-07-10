namespace Ran.Ddd.Domain.Abstraction.Entities;

public interface IEntity<TKey>
{
    TKey Id { get; set; }
    object?[] GetKeys();
}
