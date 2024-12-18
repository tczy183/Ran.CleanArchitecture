using Ran.Core.Ran.Modularity;
using Ran.Core.Ran.Modularity.Attributes;
using Ran.Ddd.Domain.Abstractions;

namespace Ran.Ddd.Domain;

[DependsOn(typeof(DomainAbstractionModule))]
public class DomainModule : BaseModule { }
