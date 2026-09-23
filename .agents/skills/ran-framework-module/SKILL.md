---
name: ran-framework-module
description: Extend or refactor reusable libraries under framework/src, including module lifecycle, dependency injection, DDD primitives, mediator, event bus, background work, EF Core, Quartz, or logging integrations. Do not use for service-only business features.
---

# Ran Framework Module

Develop framework code as a reusable library with explicit dependency and lifecycle boundaries.

## Workflow

1. Read [references/framework-map.md](references/framework-map.md) before adding a project reference, package, or module dependency.
2. Identify the lowest framework package that can own the capability. Keep abstractions free of concrete provider dependencies.
3. Follow the established split between abstraction/core packages and implementation/provider packages. Avoid moving service-specific behavior into `framework/src`.
4. For a module integration, derive from `DddModule`, express dependencies through `[DependsOn]`, and choose the lifecycle phase based on when the work must occur.
5. Prefer async lifecycle overrides for genuinely asynchronous work. Do not block on async operations or add sync wrappers unless compatibility with an existing synchronous contract requires it.
6. Preserve public API compatibility unless the user explicitly requests a breaking change. For a changed public contract, inspect all repository implementers and consumers.
7. Add package versions centrally in `Directory.Packages.props`; project files contain versionless `PackageReference` entries.
8. If adding a framework project, add it to `Ran.CleanArchitecture.slnx`, wire only necessary project references, and provide its module/global-usings pattern when applicable.
9. Validate the changed project and its direct consumers, then use `$ran-project-quality` for broader verification when available.

## Design constraints

- `Ran.Core` must not depend on higher DDD, mediator, ASP.NET Core, event-bus, or provider packages.
- Abstraction packages define contracts and lightweight models; implementations depend on abstractions, not the reverse.
- Provider-specific code belongs in a provider package, as shown by `Quartz.EntityFrameworkCore.MySql` depending on `Quartz.EntityFrameworkCore`.
- Do not expose Web/service types from framework public APIs.
- Generated XML documentation is enabled and warnings are errors; document new public APIs when analyzer settings require it and resolve warnings rather than suppressing them broadly.
