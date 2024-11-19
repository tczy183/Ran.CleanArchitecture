namespace Ran.Core.Ran.DependencyInjection;

public class ObjectAccessor<T> : IObjectAccessor<T>
{
    public ObjectAccessor(T? value = default)
    {
        Value = value;
    }

    public T? Value { get; }
}
