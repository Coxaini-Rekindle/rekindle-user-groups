using MediatR;
using Rekindle.UserGroups.Application.Common.Interfaces;
using Rekindle.UserGroups.Contracts.UserEvents;
using Rekindle.UserGroups.Domain.Entities.Users.Events;

namespace Rekindle.UserGroups.Application.Users.EventHandlers;

public class UserAvatarChangedDomainEventHandler : INotificationHandler<UserAvatarChangedDomainEvent>
{
    private readonly IEventPublisher _eventPublisher;

    public UserAvatarChangedDomainEventHandler(IEventPublisher eventPublisher)
    {
        _eventPublisher = eventPublisher;
    }

    public async Task Handle(UserAvatarChangedDomainEvent notification, CancellationToken cancellationToken)
    {
        var integrationEvent = new UserAvatarChangedEvent(notification.UserId, notification.NewAvatarFileId);

        await _eventPublisher.PublishAsync(integrationEvent);
    }
}