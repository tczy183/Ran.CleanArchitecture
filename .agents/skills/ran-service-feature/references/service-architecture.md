# Service architecture map

Read this reference when placing a service change or deciding whether a new reference is allowed.

## Existing layers

| Project | Responsibility | Existing direct project references |
| --- | --- | --- |
| `DomainAbstraction` | Domain contracts and abstractions shared without domain implementation details | `Ran.Ddd.Domain.Abstraction` |
| `Domain` | Aggregates, value objects, domain rules, domain services, and domain events | `Ran.Core`, `DomainAbstraction` |
| `ApplicationAbstraction` | Stable application-facing contracts, DTOs, and service abstractions | `Ran.Core`, `Ran.Ddd.Application.Abstraction`, `DomainAbstraction` |
| `Application` | Use-case orchestration, mediator requests/notifications and handlers | `Ran.Core`, `Ran.Ddd.Application`, `ApplicationAbstraction`, `Domain` |
| `Infrastructure` | Persistence and external-system adapters | `Application` |
| `Web` | Composition root, HTTP transport, middleware, endpoints, and host configuration | `Ran.Core.AspNetCore`, `Infrastructure`, `Application` |

These dependencies describe the repository as it exists. Preserve them unless the user explicitly asks for an architectural refactor.

## Placement decisions

- Put invariants and state transitions on aggregates/value objects in `Domain`.
- Put a domain-facing interface in `DomainAbstraction` only when an inner layer needs the contract and an outer layer will implement it.
- Put contracts intended for application consumers in `ApplicationAbstraction`; keep an internal mediator request beside its handler in `Application` when it is not a public contract.
- Put orchestration in `Application` and communicate through `IRequest<TRequest,TResponse>`, `IRequestHandler<,>`, `INotification`, or `INotificationHandler<>` as appropriate.
- Put database/provider/client implementations in `Infrastructure`.
- Put route mapping, controllers, middleware, serialization, and status-code translation in `Web`.

## Module wiring

Each layer has a module class. Express module dependencies with `[DependsOn(typeof(...))]`; the startup module is `WebModule`. Host startup calls `AddApplicationAsync<WebModule>()` followed by `InitializeApplicationAsync()`.

Use `ConfigureServices`/`ConfigureServicesAsync` for service registration and options. Use application-initialization hooks for runtime pipeline construction, not for container registration.

## Repository primitives

- Aggregate base: `Ran.Ddd.Domain.Abstraction.Entities.AggregateRoot<TKey>`
- Entity base: `Ran.Ddd.Domain.Abstraction.Entities.Entity<TKey>`
- Value-object base: `Ran.Ddd.Domain.Abstraction.Entities.ValueObject`
- Local/distributed events: protected methods on `AggregateRoot<TKey>`
- Mediator entry point: `Ran.Mediator.IMediator`
- Conventional DI lifetime markers: `ITransientDependency`, `IScopedDependency`, and `ISingletonDependency`
