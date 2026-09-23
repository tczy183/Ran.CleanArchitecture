using Ran.Core.Modularity;
using Ran.Ddd.Application;

namespace Ran.Basic;

[DependsOn(typeof(BasicDomainModule), typeof(DddApplicationModule))]
public sealed class BasicApplicationModule : DddModule;
