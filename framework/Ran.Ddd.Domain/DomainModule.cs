using Ran.Core.Modularity;
using Ran.Ddd.Domain.Abstractions;

namespace Ran.Ddd.Domain;

[DependsOn(typeof(DomainAbstractionModule))]
public class DomainModule : BaseModule { }
