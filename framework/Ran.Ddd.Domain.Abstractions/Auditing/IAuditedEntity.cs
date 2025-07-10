using Ran.Ddd.Domain.Abstractions.Entities;

namespace Ran.Ddd.Domain.Abstractions.Auditing;

public interface IAuditedEntity<TKey> : IEntity<TKey>
{
    TKey CreatedBy { get; set; }
    DateTime CreatedAt { get; set; }
    TKey? LastModifiedBy { get; set; }
    DateTime? LastModifiedAt { get; set; }
}
