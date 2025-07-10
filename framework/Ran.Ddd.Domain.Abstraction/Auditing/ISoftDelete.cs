using Ran.Ddd.Domain.Abstraction.Entities;

namespace Ran.Ddd.Domain.Abstraction.Auditing;

public interface ISoftDelete<TKey> : IEntity<TKey>
{
    bool IsDeleted { get; }
    DateTime? DeletedAt { get; }
    TKey? DeletedBy { get; }
}
