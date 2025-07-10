namespace ApplicationAbstraction;

[DependsOn(
    typeof(DomainAbstractionModule),
    typeof(DddApplicationAbstractionModule)
)]
public class ApplicationAbstractionModule : DddModule
{
}
