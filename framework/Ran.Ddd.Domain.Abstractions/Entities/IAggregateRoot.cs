namespace Ran.Ddd.Domain.Abstractions.Entities;

public interface IAggregateRoot<TKey>
    : IEntity<TKey>,
        IHasConcurrencyStamp,
        IGeneratesDomainEvents { }
