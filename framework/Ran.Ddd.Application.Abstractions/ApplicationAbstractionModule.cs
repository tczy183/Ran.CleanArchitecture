using Ran.Core.Modularity;
using Ran.Ddd.Domain.Abstractions;

namespace Ran.Ddd.Application.Abstractions;

[DependsOn(typeof(DomainAbstractionModule))]
public class ApplicationAbstractionModule : BaseModule
{
}
