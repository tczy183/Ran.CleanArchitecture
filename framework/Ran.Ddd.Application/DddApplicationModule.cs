using Ran.Core.Modularity;
using Ran.Ddd.Domain;
using Ran.Ddd.Domain.Abstraction;

namespace Ran.Ddd.Application;


[DependsOn(typeof(DddDomainModule),typeof(DddDomainAbstractionModule))]
public class DddApplicationModule : DddModule
{
}
