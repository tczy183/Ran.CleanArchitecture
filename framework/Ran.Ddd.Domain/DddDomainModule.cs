using Ran.Core.Modularity;
using Ran.Ddd.Domain.Abstraction;

namespace Ran.Ddd.Domain;

[DependsOn(typeof(DddDomainAbstractionModule))]
public class DddDomainModule : DddModule 
{ }
