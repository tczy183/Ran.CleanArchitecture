using Application;
using Ran.Core.Ran.Modularity;
using Ran.Core.Ran.Modularity.Attributes;

namespace Infrastructure;

[DependsOn(typeof(ApplicationModule))]
public class InfrastructureModule : BaseModule { }
