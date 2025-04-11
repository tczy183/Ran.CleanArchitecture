using Ran.Core.Modularity;
using Ran.Ddd.Domain;

namespace Ran.Ddd.Application;


[DependsOn(typeof(DomainModule))]
public class ApplicationModule : BaseModule
{
}
