using Ran.Core.Modularity;
using Ran.Ddd.EntityFramework;

namespace Ran.BackgroundJob.EntityFrameworkCore;

[DependsOn(typeof(BackgroundJobModule), typeof(EntityFrameworkModule))]
public sealed class BackgroundJobEntityFrameworkCoreModule : DddModule { }
