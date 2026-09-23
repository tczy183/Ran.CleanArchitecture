# Framework dependency and lifecycle map

Read this reference when selecting a framework package or wiring a module.

## Package topology

- `Ran.Core`: application bootstrap, modularity, conventional DI, options, localization, exceptions, reflection, threading, and shared utilities.
- `Ran.Core.AspNetCore` -> `Ran.Core`: ASP.NET Core hosting and application-builder integration.
- `Ran.Ddd.Domain.Abstraction` -> `Ran.Core`: entities, aggregate roots, value objects, repository/unit-of-work contracts, auditing, and tenancy contracts.
- `Ran.Ddd.Domain` -> `Ran.Ddd.Domain.Abstraction`: domain-service implementation layer.
- `Ran.Ddd.Application.Abstraction` -> `Ran.Ddd.Domain.Abstraction`: application-service contracts.
- `Ran.Ddd.Application` -> application abstraction, domain, and `Ran.Mediator`: application-service implementation layer.
- `Ran.Ddd.EntityFramework` -> `Ran.Ddd.Domain` plus EF Core: EF repositories and DbContext base.
- `Ran.Mediator` -> `Ran.Core`: request, notification, stream, and pipeline dispatch.
- `Ran.EventBus.Abstractions` -> `Ran.Core`; `Ran.EventBus` -> its abstractions and `Ran.BackgroundWorker`.
- `Ran.BackgroundWorker` -> `Ran.Core`; `Ran.BackgroundJob` -> `Ran.BackgroundWorker` and `Ran.Core`.
- `modules/background-jobs/Ran.BackgroundJob.EntityFrameworkCore` -> `Ran.BackgroundJob` and `Ran.Ddd.EntityFramework`: optional durable job storage kept outside the provider-neutral framework package.
- `modules/basic`: a single-tenant business module split into Domain, Application, EF Core, and ASP.NET Core adapters; keep user/role/menu/permission and department data-scope behavior here rather than in `Ran.Core`.
- `Quartz.EntityFrameworkCore.MySql` -> `Quartz.EntityFrameworkCore`: provider-neutral model separated from MySQL configuration.
- `Ran.Serilog.Sinks.Mysql`: standalone MySQL Serilog sink integration.

Avoid dependency cycles. When two packages appear to need each other, extract the shared contract downward rather than adding reciprocal references.

## Module lifecycle

`DddModule` exposes these phases in order:

1. `PreConfigureServices[Async]`
2. `ConfigureServices[Async]`
3. `PostConfigureServices[Async]`
4. `OnPreApplicationInitialization[Async]`
5. `OnApplicationInitialization[Async]`
6. `OnPostApplicationInitialization[Async]`
7. `OnApplicationShutdown[Async]`

Use service phases to construct the container. Use initialization phases for runtime startup and middleware. Use shutdown for owned-resource cleanup. A synchronous override is sufficient when all operations are synchronous.

## Registration conventions

The default registrar discovers lifetime markers and dependency metadata, then exposes service types according to the repository's DI attributes/conventions. Before writing manual registrations, inspect:

- `Ran.Core/DependencyInjection/ServiceLifetimes`
- `DependencyAttribute`
- `ExposeServicesAttribute` and keyed-service variants
- `DefaultConventionalRegistrar`

Manual registration remains appropriate for factories, options-bound instances, third-party services, or registrations whose semantics are not representable by conventions.
