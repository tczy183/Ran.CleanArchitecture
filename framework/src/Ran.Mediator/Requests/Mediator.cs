﻿using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Ran.Mediator.Exceptions;
using Ran.Mediator.Requests.Notification;
using Ran.Mediator.Requests.Send;
using Ran.Mediator.Requests.Stream;

namespace Ran.Mediator.Requests;

public sealed class Mediator(IServiceProvider serviceProvider) : IMediator
{
    public TResponse Send<TRequest, TResponse>(
        IRequest<TRequest, TResponse> request,
        CancellationToken cancellationToken
    )
        where TRequest : class, IRequest
    {
        try
        {
            return serviceProvider
                .GetRequiredService<IRequestHandler<TRequest, TResponse>>()
                .Handle(Unsafe.As<TRequest>(request), cancellationToken);
        }
        catch (Exception e)
            when (e.Message.Contains("No service for type", StringComparison.OrdinalIgnoreCase))
        {
            throw new HandlerNotFoundException<TRequest, TResponse>();
        }
    }

    public IAsyncEnumerable<TResponse> CreateStream<TRequest, TResponse>(
        IStreamRequest<TRequest, TResponse> request,
        CancellationToken cancellationToken
    )
        where TRequest : class, IStreamRequest
    {
        return serviceProvider
            .GetRequiredService<IStreamRequestHandler<TRequest, TResponse>>()
            .Handle(Unsafe.As<TRequest>(request), cancellationToken);
    }

    public async ValueTask Publish<TNotification>(
        TNotification request,
        CancellationToken cancellationToken
    )
        where TNotification : INotification
    {
        var notificationsInDi = serviceProvider.GetRequiredService<
            IEnumerable<INotificationHandler<TNotification>>
        >();

        var notifications = Unsafe.As<INotificationHandler<TNotification>[]>(notificationsInDi);
        foreach (var notification in notifications)
        {
            var valueTask = notification.Handle(request, cancellationToken);
            if (!valueTask.IsCompletedSuccessfully)
            {
                await valueTask;
            }
        }
    }
}
