using MediatR;
using Rekindle.UserGroups.Application.Common.Interfaces;
using Rekindle.UserGroups.Contracts.GroupEvents;
using Rekindle.UserGroups.Domain.Entities.Groups.Events;

namespace Rekindle.UserGroups.Application.Groups.EventHandlers;

/// <summary>
/// Handles the GroupDeletedDomainEvent and publishes the corresponding integration event
/// </summary>
public class GroupDeletedDomainEventHandler : INotificationHandler<GroupDeletedDomainEvent>
{
    private readonly IEventPublisher _eventPublisher;
    
    public GroupDeletedDomainEventHandler(IEventPublisher eventPublisher)
    {
        _eventPublisher = eventPublisher;
    }
    
    public async Task Handle(GroupDeletedDomainEvent notification, CancellationToken cancellationToken)
    {
        // Map the domain event to an integration event
        var integrationEvent = new GroupDeletedEvent(
            notification.GroupId,
            notification.Name,
            notification.Description,
            notification.DeletedByUserId,
            notification.DeletedAt);
            
        // Publish the integration event to the message bus
        await _eventPublisher.PublishAsync(integrationEvent);
    }
}
