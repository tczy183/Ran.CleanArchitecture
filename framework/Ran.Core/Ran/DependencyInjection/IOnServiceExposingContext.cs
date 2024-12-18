namespace Ran.Core.Ran.DependencyInjection;

public interface IOnServiceExposingContext
{
    Type ImplementationType { get; }

    List<ServiceIdentifier> ExposedTypes { get; }
}
