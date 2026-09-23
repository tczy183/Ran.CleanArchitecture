using Ran.Core.Modularity;
using Ran.Ddd.EntityFramework;

namespace Ran.Basic.EntityFrameworkCore;

[DependsOn(typeof(BasicDomainModule), typeof(EntityFrameworkModule))]
public sealed class BasicEntityFrameworkCoreModule : DddModule;
