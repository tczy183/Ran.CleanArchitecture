using Ran.Ddd.Domain.Abstraction.Entities;

namespace Ran.Ddd.Domain.Abstraction.Auditing;

public interface IAuditedEntity<TKey> : IEntity<TKey>
{
    TKey CreatedBy { get; set; }
    DateTime CreatedAt { get; set; }
    TKey? LastModifiedBy { get; set; }
    DateTime? LastModifiedAt { get; set; }
}
