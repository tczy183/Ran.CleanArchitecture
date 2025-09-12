using Ran.Ddd.Domain.Abstraction.Entities;

namespace Ran.Ddd.Domain.Abstraction.Auditing;

public class AuditedEntity<TKey>(TKey createdBy, bool isDeleted = false)
    : Entity<TKey>,
        IAuditedEntity<TKey>,
        ISoftDelete<TKey>
{
    public TKey CreatedBy { get; set; } = createdBy;
    public DateTime CreatedAt { get; set; }

    public TKey? LastModifiedBy { get; set; }
    public DateTime? LastModifiedAt { get; set; }

    public bool IsDeleted { get; } = isDeleted;
    public DateTime? DeletedAt { get; set; }
    public TKey? DeletedBy { get; set; }
}
