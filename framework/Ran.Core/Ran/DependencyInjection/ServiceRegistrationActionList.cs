using Microsoft.Extensions.DependencyInjection;

namespace Ran.Core.Ran.DependencyInjection;

public class ServiceRegistrationActionList : List<Action<IOnServiceRegistredContext>>
{
    public bool IsClassInterceptorsDisabled { get; set; }
}
