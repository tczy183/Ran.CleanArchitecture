namespace Ran.Basic.Authorization;

public interface IDataPermissionEntity
{
    Guid OwnerUserId { get; }

    Guid? DepartmentId { get; }
}
