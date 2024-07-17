using Application.Common.Interfaces;

using Domain.Common;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Infrastructure.EntityFrameworkCore.Interceptors;


public class AuditableEntityInterceptor(
    IUser user,
    TimeProvider dateTime) : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public void UpdateEntities(DbContext? context)
    {
        if (context == null) return;
        

        foreach (var entry in context.ChangeTracker.Entries())
        {
            if (entry.State is EntityState.Added or EntityState.Modified || entry.HasChangedOwnedEntities())
            {
                var entityType = entry.Entity.GetType();
                // 检查实体类型是否继承自 BaseEntity<>
                var baseType = entityType.BaseType;
                if (baseType != null && baseType.IsGenericType &&
                    baseType.GetGenericTypeDefinition() == typeof(BaseAuditableEntity<>))
                {
                    var utcNow = dateTime.GetUtcNow();

                    // 使用反射获取并设置 CreatedBy 和 Created 属性
                    if (entry.State == EntityState.Added)
                    {
                        var createdByProperty = entityType.GetProperty("CreatedBy");
                        if (createdByProperty != null)
                        {
                            createdByProperty.SetValue(entry.Entity, user.Id);
                        }

                        var createdProperty = entityType.GetProperty("Created");
                        if (createdProperty != null)
                        {
                            createdProperty.SetValue(entry.Entity, utcNow);
                        }
                    }

                    // 使用反射获取并设置 LastModifiedBy 和 LastModified 属性
                    var lastModifiedByProperty = entityType.GetProperty("LastModifiedBy");
                    if (lastModifiedByProperty != null)
                    {
                        lastModifiedByProperty.SetValue(entry.Entity, user.Id);
                    }

                    var lastModifiedProperty = entityType.GetProperty("LastModified");
                    if (lastModifiedProperty != null)
                    {
                        lastModifiedProperty.SetValue(entry.Entity, utcNow);
                    }
                }

            }
        }
    }
}

public static class Extensions
{
    public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
        entry.References.Any(r => 
            r.TargetEntry != null && 
            r.TargetEntry.Metadata.IsOwned() && 
            (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));
}
