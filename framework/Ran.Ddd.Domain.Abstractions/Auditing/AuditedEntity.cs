using Ran.Ddd.Domain.Abstractions.Entities;

namespace Ran.Ddd.Domain.Abstractions.Auditing;

public class AuditedEntity<TKey> : Entity<TKey>, IAuditedEntity<TKey>, ISoftDelete<TKey>
{
    public AuditedEntity(TKey createdBy, bool isDeleted = false)
    {
        CreatedBy = createdBy;
        IsDeleted = isDeleted;
    }

    public TKey CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }

    public TKey? LastModifiedBy { get; set; }
    public DateTime? LastModifiedAt { get; set; }

    public bool IsDeleted { get; }
    public DateTime? DeletedAt { get; }
    public TKey? DeletedBy { get; }
}

public interface IAuditedEntity<TKey> : IEntity<TKey>
{
    TKey CreatedBy { get; set; }
    DateTime CreatedAt { get; set; }
    TKey? LastModifiedBy { get; set; }
    DateTime? LastModifiedAt { get; set; }
}

public interface ISoftDelete<TKey> : IEntity<TKey>
{
    bool IsDeleted { get; }
    DateTime? DeletedAt { get; }
    TKey? DeletedBy { get; }
}
