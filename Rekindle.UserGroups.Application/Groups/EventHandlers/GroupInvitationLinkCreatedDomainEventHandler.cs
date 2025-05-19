using MediatR;
using Rekindle.UserGroups.Application.Common.Interfaces;
using Rekindle.UserGroups.Contracts;
using Rekindle.UserGroups.Contracts.GroupEvents;
using Rekindle.UserGroups.Domain.Entities.GroupInvites.Events;

namespace Rekindle.UserGroups.Application.Groups.EventHandlers;

/// <summary>
/// Handles the GroupInvitationLinkCreatedDomainEvent and publishes the corresponding integration event
/// </summary>
public class GroupInvitationLinkCreatedDomainEventHandler : INotificationHandler<GroupInvitationLinkCreatedDomainEvent>
{
    private readonly IEventPublisher _eventPublisher;
    
    public GroupInvitationLinkCreatedDomainEventHandler(IEventPublisher eventPublisher)
    {
        _eventPublisher = eventPublisher;
    }
    
    public async Task Handle(GroupInvitationLinkCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        // Map the domain event to an integration event
        var integrationEvent = new GroupInvitationLinkCreatedEvent(
            notification.InvitationLinkId,
            notification.GroupId,
            notification.CreatedByUserId,
            notification.Token,
            notification.MaxUses,
            notification.ExpiresAt);
            
        // Publish the integration event to the message bus
        await _eventPublisher.PublishAsync(integrationEvent);
    }
}
