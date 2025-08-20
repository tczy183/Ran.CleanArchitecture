using Ran.Core.Modularity;
using Ran.Ddd.Domain.Abstraction;

namespace Ran.Ddd.Application.Abstraction;

[DependsOn(typeof(DddDomainAbstractionModule))]
public class DddApplicationAbstractionModule : DddModule { }
