using Ran.Core.Ran.Modularity;
using Ran.Core.Ran.Modularity.Attributes;
using Ran.Ddd.Domain;

namespace Ran.Ddd.EntityFramework;

[DependsOn(typeof(DomainModule))]
public class EntityFrameworkModule : BaseModule { }
