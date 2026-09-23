namespace Ran.Basic.Authorization;

public static class DataPermissionQueryableExtensions
{
    public static IQueryable<TEntity> WhereDataPermission<TEntity>(
        this IQueryable<TEntity> source,
        DataPermission permission
    )
        where TEntity : class, IDataPermissionEntity
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(permission);

        if (permission.AllowsAll)
        {
            return source;
        }

        var allowsSelf = permission.AllowsSelf;
        var userId = permission.UserId;
        var departmentIds = permission.DepartmentIds.ToArray();
        if (!allowsSelf && departmentIds.Length == 0)
        {
            return source.Where(_ => false);
        }

        return source.Where(entity =>
            allowsSelf && entity.OwnerUserId == userId
            || entity.DepartmentId.HasValue && departmentIds.Contains(entity.DepartmentId.Value)
        );
    }
}
