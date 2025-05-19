using MediatR;
using Rekindle.UserGroups.Application.Common.Interfaces;
using Rekindle.UserGroups.Contracts.GroupEvents;
using Rekindle.UserGroups.Domain.Entities.Groups.Events;

namespace Rekindle.UserGroups.Application.Groups.EventHandlers;

/// <summary>
/// Handles the GroupCreatedDomainEvent and publishes the corresponding integration event
/// </summary>
public class GroupCreatedDomainEventHandler : INotificationHandler<GroupCreatedDomainEvent>
{
    private readonly IEventPublisher _eventPublisher;
    
    public GroupCreatedDomainEventHandler(IEventPublisher eventPublisher)
    {
        _eventPublisher = eventPublisher;
    }
    
    public async Task Handle(GroupCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        // Map the domain event to an integration event
        var integrationEvent = new GroupCreatedEvent(
            notification.GroupId,
            notification.Name,
            notification.Description,
            notification.CreatedByUserId,
            notification.CreatedAt);
            
        // Publish the integration event to the message bus
        await _eventPublisher.PublishAsync(integrationEvent);
    }
}
