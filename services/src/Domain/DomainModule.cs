using DomainAbstraction;

namespace Domain;

[DependsOn(typeof(DddDomainAbstractionModule), typeof(DomainAbstractionModule))]
public class DomainModule : DddModule { }
