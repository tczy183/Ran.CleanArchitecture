namespace Ran.Dependencies.Geterators;

public record ServiceRegistration(
    string Lifetime,
    string ImplementationType,
    EquatableArray<string> ServiceTypes,
    string? ServiceKey,
    string? Factory,
    string Duplicate,
    string Registration,
    EquatableArray<string> Tags
)
{
    public string Lifetime { get; } = Lifetime;
    public string ImplementationType { get; } = ImplementationType;
    public EquatableArray<string> ServiceTypes { get; } = ServiceTypes;
    public string? ServiceKey { get; } = ServiceKey;
    public string? Factory { get; } = Factory;
    public string Duplicate { get; } = Duplicate;
    public string Registration { get; } = Registration;
    public EquatableArray<string> Tags { get; } = Tags;
}
