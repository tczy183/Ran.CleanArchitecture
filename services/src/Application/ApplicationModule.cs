namespace Application;

[DependsOn(
    typeof(DomainModule),
    typeof(DddApplicationModule),
    typeof(ApplicationAbstractionModule)
)]
public class ApplicationModule : DddModule { }
