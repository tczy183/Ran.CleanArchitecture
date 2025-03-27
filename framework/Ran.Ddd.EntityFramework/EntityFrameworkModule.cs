using Ran.Core.Modularity;
using Ran.Ddd.Domain;

namespace Ran.Ddd.EntityFramework;

[DependsOn(typeof(DomainModule))]
public class EntityFrameworkModule : BaseModule { }
