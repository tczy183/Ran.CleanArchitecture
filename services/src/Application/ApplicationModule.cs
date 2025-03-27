using Domain;
using Ran.Core.Modularity;

namespace Application;

[DependsOn(typeof(DomainModule))]
public class ApplicationModule : BaseModule { }
