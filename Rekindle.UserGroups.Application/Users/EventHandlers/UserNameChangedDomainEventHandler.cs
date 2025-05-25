using MediatR;
using Rekindle.UserGroups.Application.Common.Interfaces;
using Rekindle.UserGroups.Contracts.UserEvents;
using Rekindle.UserGroups.Domain.Entities.Users.Events;

namespace Rekindle.UserGroups.Application.Users.EventHandlers;

public class UserNameChangedDomainEventHandler : INotificationHandler<UserNameChangedDomainEvent>
{
    private readonly IEventPublisher _eventPublisher;

    public UserNameChangedDomainEventHandler(IEventPublisher eventPublisher)
    {
        _eventPublisher = eventPublisher;
    }

    public async Task Handle(UserNameChangedDomainEvent notification, CancellationToken cancellationToken)
    {
        var integrationEvent = new UserNameChangedEvent(notification.UserId, notification.NewName);

        await _eventPublisher.PublishAsync(integrationEvent);
    }
}