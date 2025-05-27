using MediatR;
using Rekindle.UserGroups.Application.Common.Interfaces;
using Rekindle.UserGroups.Contracts.GroupEvents;
using Rekindle.UserGroups.Domain.Entities.Groups.Events;

namespace Rekindle.UserGroups.Application.Groups.EventHandlers;

/// <summary>
/// Handles the UserJoinedGroupDomainEvent and publishes the corresponding integration event
/// </summary>
public class UserJoinedGroupDomainEventHandler : INotificationHandler<UserJoinedGroupDomainEvent>
{
    private readonly IEventPublisher _eventPublisher;
    
    public UserJoinedGroupDomainEventHandler(IEventPublisher eventPublisher)
    {
        _eventPublisher = eventPublisher;
    }
    
    public async Task Handle(UserJoinedGroupDomainEvent notification, CancellationToken cancellationToken)
    {
        // Map the domain event to an integration event
        // Note: We convert the role enum to a string for the integration event
        var integrationEvent = new UserJoinedGroupEvent(
            notification.GroupId,
            notification.UserId,
            notification.UserName,
            notification.Name,
            notification.AvatarFileId,
            notification.Role.ToString(),
            notification.JoinedAt);
            
        // Publish the integration event to the message bus
        await _eventPublisher.PublishAsync(integrationEvent);
    }
}
