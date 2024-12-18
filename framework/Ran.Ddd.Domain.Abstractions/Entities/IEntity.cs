namespace Ran.Ddd.Domain.Abstractions.Entities;

public interface IEntity<TKey>
{
    TKey Id { get; set; }
    object?[] GetKeys();
}
