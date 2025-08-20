using Ran.Mediator.Requests.Send;

namespace Ran.Mediator.Requests.Stream;

public interface IStreamRequestHandler<in TRequest, out TResponse> : IRequestHandler
    where TRequest : class, IStreamRequest
{
    IAsyncEnumerable<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}
