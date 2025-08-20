using Ran.Mediator.Requests.Notification;
using Ran.Mediator.Requests.Send;
using Ran.Mediator.Requests.Stream;

namespace Ran.Mediator.Requests;

public interface IMediator
{
    TResponse Send<TRequest, TResponse>(
        IRequest<TRequest, TResponse> request,
        CancellationToken cancellationToken
    )
        where TRequest : class, IRequest;

    IAsyncEnumerable<TResponse> CreateStream<TRequest, TResponse>(
        IStreamRequest<TRequest, TResponse> request,
        CancellationToken cancellationToken
    )
        where TRequest : class, IStreamRequest;

    ValueTask Publish<TNotification>(TNotification request, CancellationToken cancellationToken)
        where TNotification : INotification;
}
