using Ran.Core.Modularity;
using Ran.Ddd.Domain;

namespace Ran.Ddd.EntityFramework;

[DependsOn(typeof(DddDomainModule))]
public class EntityFrameworkModule : DddModule { }
