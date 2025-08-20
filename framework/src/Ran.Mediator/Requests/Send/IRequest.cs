﻿namespace Ran.Mediator.Requests.Send;

public interface IRequest;

public interface IRequest<TRequest, TResponse> : IRequest
    where TRequest : class;
