namespace Ran.Dependencies.Geterators;

public record ModuleRegistration(
    string ClassName,
    string MethodName,
    bool IsStatic,
    bool HasTagCollection
)
{
    public string ClassName { get; } = ClassName;
    public string MethodName { get; } = MethodName;
    public bool IsStatic { get; } = IsStatic;
    public bool HasTagCollection { get; } = HasTagCollection;
}
