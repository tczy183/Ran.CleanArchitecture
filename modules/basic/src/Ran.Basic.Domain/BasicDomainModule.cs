using Ran.Core.Modularity;
using Ran.Ddd.Domain.Abstraction;

namespace Ran.Basic;

[DependsOn(typeof(DddDomainAbstractionModule))]
public sealed class BasicDomainModule : DddModule;
