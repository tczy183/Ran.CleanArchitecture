namespace Ran.Core.Ran.DependencyInjection;

public interface IObjectAccessor<out T>
{
    T? Value { get; }
}
