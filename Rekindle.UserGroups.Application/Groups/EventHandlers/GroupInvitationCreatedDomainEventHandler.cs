using MediatR;
using Rekindle.UserGroups.Application.Common.Interfaces;
using Rekindle.UserGroups.Contracts.GroupEvents;
using Rekindle.UserGroups.Domain.Entities.GroupInvites.Events;

namespace Rekindle.UserGroups.Application.Groups.EventHandlers;

/// <summary>
/// Handles the GroupInvitationCreatedDomainEvent and publishes the corresponding integration event
/// </summary>
public class GroupInvitationCreatedDomainEventHandler : INotificationHandler<GroupInvitationCreatedDomainEvent>
{
    private readonly IEventPublisher _eventPublisher;
    
    public GroupInvitationCreatedDomainEventHandler(IEventPublisher eventPublisher)
    {
        _eventPublisher = eventPublisher;
    }
    
    public async Task Handle(GroupInvitationCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        // Map the domain event to an integration event
        var integrationEvent = new GroupInvitationCreatedEvent(
            notification.InvitationId,
            notification.GroupId,
            notification.InvitedByUserId,
            notification.InvitedUserId,
            notification.Email,
            notification.ExpiresAt);
            
        // Publish the integration event to the message bus
        await _eventPublisher.PublishAsync(integrationEvent);
    }
}
