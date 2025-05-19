using MediatR;
using Rekindle.UserGroups.Application.Common.Interfaces;
using Rekindle.UserGroups.Contracts.GroupEvents;
using Rekindle.UserGroups.Domain.Entities.Groups.Events;

namespace Rekindle.UserGroups.Application.Groups.EventHandlers;

/// <summary>
/// Handles the GroupUpdatedDomainEvent and publishes the corresponding integration event
/// </summary>
public class GroupUpdatedDomainEventHandler : INotificationHandler<GroupUpdatedDomainEvent>
{
    private readonly IEventPublisher _eventPublisher;
    
    public GroupUpdatedDomainEventHandler(IEventPublisher eventPublisher)
    {
        _eventPublisher = eventPublisher;
    }
    
    public async Task Handle(GroupUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        // Map the domain event to an integration event
        var integrationEvent = new GroupUpdatedEvent(
            notification.GroupId,
            notification.Name,
            notification.Description,
            notification.UpdatedByUserId);
            
        // Publish the integration event to the message bus
        await _eventPublisher.PublishAsync(integrationEvent);
    }
}
