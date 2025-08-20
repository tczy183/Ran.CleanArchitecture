﻿using System.Runtime.CompilerServices;

namespace Ran.Mediator.Requests.Send;

public interface IPipelineBehavior<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequest : class, IRequest<TRequest, TResponse>
{
    IRequestHandler<TRequest, TResponse> NextPipeline { get; set; }

    void IRequestHandler.SetNext(object handler)
    {
        NextPipeline = Unsafe.As<IRequestHandler<TRequest, TResponse>>(handler);
    }
}
