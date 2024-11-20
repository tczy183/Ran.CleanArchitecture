namespace Ran.Dependencies.Geterators;

public record MethodOptions(string? Name, string? Internal)
{
    public string? Name { get; } = Name;
    public string? Internal { get; } = Internal;
}
