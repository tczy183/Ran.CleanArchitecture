using Ran.Ddd.Domain.Abstractions.Entities;

namespace Ran.Ddd.Domain.Abstractions.Auditing;

public interface ISoftDelete<TKey> : IEntity<TKey>
{
    bool IsDeleted { get; }
    DateTime? DeletedAt { get; }
    TKey? DeletedBy { get; }
}
