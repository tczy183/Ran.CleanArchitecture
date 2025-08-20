using Ran.Core.Modularity;
using Ran.Ddd.Domain;
using Ran.Ddd.Domain.Abstraction;
using Ran.Mediator;

namespace Ran.Ddd.Application;

[DependsOn(typeof(DddDomainModule), typeof(DddDomainAbstractionModule), typeof(MediatorModule))]
public class DddApplicationModule : DddModule { }
