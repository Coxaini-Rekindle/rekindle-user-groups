using MediatR;
using Rekindle.UserGroups.Application.Common.Interfaces;
using Rekindle.UserGroups.Contracts.UserEvents;
using Rekindle.UserGroups.Domain.Entities.Users.Events;

namespace Rekindle.UserGroups.Application.Users.EventHandlers;

public class UserCreatedDomainEventHandler : INotificationHandler<UserCreatedDomainEvent>
{
    private readonly IEventPublisher _eventPublisher;

    public UserCreatedDomainEventHandler(IEventPublisher eventPublisher)
    {
        _eventPublisher = eventPublisher;
    }

    public async Task Handle(UserCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var integrationEvent = new UserCreatedEvent(notification.UserId, notification.Email, notification.Name,
            notification.Username);

        await _eventPublisher.PublishAsync(integrationEvent);
    }
}