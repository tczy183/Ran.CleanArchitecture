using Domain;
using Ran.Core.Ran.Modularity;
using Ran.Core.Ran.Modularity.Attributes;

namespace Application;

[DependsOn(typeof(DomainModule))]
public class ApplicationModule : BaseModule { }
