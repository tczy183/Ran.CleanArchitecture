using Microsoft.CodeAnalysis;

namespace Ran.Dependencies.Geterators;

public record ServiceRegistrationContext(
    EquatableArray<Diagnostic>? Diagnostics = null,
    EquatableArray<ServiceRegistration>? ServiceRegistrations = null,
    EquatableArray<ModuleRegistration>? ModuleRegistrations = null
)
{
    public EquatableArray<Diagnostic>? Diagnostics { get; } = Diagnostics;
    public EquatableArray<ServiceRegistration>? ServiceRegistrations { get; } =
        ServiceRegistrations;
    public EquatableArray<ModuleRegistration>? ModuleRegistrations { get; } = ModuleRegistrations;
}
