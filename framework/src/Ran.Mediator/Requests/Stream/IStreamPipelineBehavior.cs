using System.Runtime.CompilerServices;
using Ran.Mediator.Requests.Send;

namespace Ran.Mediator.Requests.Stream;

public interface IStreamPipelineBehavior<TRequest, TResponse>
    : IStreamRequestHandler<TRequest, TResponse>
    where TRequest : class, IStreamRequest<TRequest, TResponse>
{
    IStreamRequestHandler<TRequest, TResponse> NextPipeline { get; set; }

    void IRequestHandler.SetNext(object handler)
    {
        NextPipeline = Unsafe.As<IStreamRequestHandler<TRequest, TResponse>>(handler);
    }
}
