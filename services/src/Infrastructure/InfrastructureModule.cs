using Application;
using Ran.Core.Modularity;

namespace Infrastructure;

[DependsOn(typeof(ApplicationModule))]
public class InfrastructureModule : DddModule { }
