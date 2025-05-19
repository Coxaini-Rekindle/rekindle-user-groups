using MediatR;
using Rekindle.UserGroups.Application.Common.Interfaces;
using Rekindle.UserGroups.Contracts.GroupEvents;
using Rekindle.UserGroups.Domain.Entities.GroupInvites.Events;

namespace Rekindle.UserGroups.Application.Groups.EventHandlers;

/// <summary>
/// Handles the GroupInvitationAcceptedDomainEvent and publishes the corresponding integration event
/// </summary>
public class GroupInvitationAcceptedDomainEventHandler : INotificationHandler<GroupInvitationAcceptedDomainEvent>
{
    private readonly IEventPublisher _eventPublisher;

    public GroupInvitationAcceptedDomainEventHandler(IEventPublisher eventPublisher)
    {
        _eventPublisher = eventPublisher;
    }

    public async Task Handle(GroupInvitationAcceptedDomainEvent notification, CancellationToken cancellationToken)
    {
        // Map the domain event to an integration event
        var integrationEvent = new GroupInvitationAcceptedEvent(
            notification.InvitationId,
            notification.GroupId,
            notification.UserId);

        // Publish the integration event to the message bus
        await _eventPublisher.PublishAsync(integrationEvent);
    }
}