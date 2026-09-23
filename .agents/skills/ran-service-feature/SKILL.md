---
name: ran-service-feature
description: Implement or change business capabilities in the services solution while preserving this repository's DDD and Clean Architecture layer boundaries. Use for domain models, application requests and handlers, infrastructure adapters, or Web endpoints; do not use for reusable framework internals.
---

# Ran Service Feature

Implement a service feature as the smallest coherent vertical slice across `services/src`.

## Workflow

1. Read [references/service-architecture.md](references/service-architecture.md) before deciding file placement or adding project references.
2. Trace a nearby feature and the relevant `*Module` classes before editing. Treat current code as evidence of local conventions, but do not copy obvious formatting or correctness defects.
3. Put each responsibility in the innermost layer that can own it. Add outward-layer wiring only where the feature actually needs it.
4. Preserve the project-reference direction already encoded in the `.csproj` files. Do not solve access problems by making an inner layer reference `Infrastructure` or `Web`.
5. Prefer the repository's mediator abstractions for request/notification dispatch and its conventional dependency injection where applicable. Register services explicitly only when conventions cannot express the requirement.
6. Update the owning module's `[DependsOn]`, service configuration, or application initialization only when the new behavior introduces a real module dependency or host concern.
7. Add focused tests under `services/test` when a test project exists or when the requested change includes establishing one. Do not silently introduce a new testing stack for an unrelated feature.
8. Validate the affected projects first, then use `$ran-project-quality` for repository-level verification when available.

## Boundaries

- Keep domain behavior independent of ASP.NET Core routing, logging sinks, configuration files, and persistence implementations.
- Keep HTTP concerns in `Web`; endpoints translate transport input/output and delegate behavior rather than implementing business rules.
- Keep adapter and persistence details in `Infrastructure`.
- Avoid broad framework changes for a single service feature. If a capability is genuinely reusable across services, switch to `$ran-framework-module` and state that scope change.
- Do not add NuGet versions to individual projects; this repository uses central package management in `Directory.Packages.props`.
