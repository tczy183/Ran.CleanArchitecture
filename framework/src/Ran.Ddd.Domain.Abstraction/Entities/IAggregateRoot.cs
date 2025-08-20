namespace Ran.Ddd.Domain.Abstraction.Entities;

public interface IAggregateRoot<TKey>
    : IEntity<TKey>,
        IHasConcurrencyStamp,
        IGeneratesDomainEvents { }
