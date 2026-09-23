namespace Ran.Basic.Authorization;

public enum DataScope
{
    None = 0,
    Self = 10,
    Department = 20,
    DepartmentAndChildren = 30,
    CustomDepartments = 40,
    All = 50,
}
