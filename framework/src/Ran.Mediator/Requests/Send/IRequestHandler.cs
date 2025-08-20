using System.Diagnostics.CodeAnalysis;

namespace Ran.Mediator.Requests.Send;

public interface IRequestHandler
{
    [ExcludeFromCodeCoverage]
    internal void SetNext(object handler) { }
}

public interface IRequestHandler<in TRequest, out TResponse> : IRequestHandler
    where TRequest : class, IRequest
{
    TResponse Handle(TRequest request, CancellationToken cancellationToken);
}
