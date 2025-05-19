using MediatR;
using Rekindle.UserGroups.Application.Common.Interfaces;
using Rekindle.UserGroups.Contracts.GroupEvents;
using Rekindle.UserGroups.Domain.Entities.Groups.Events;

namespace Rekindle.UserGroups.Application.Groups.EventHandlers;

/// <summary>
/// Handles the UserLeftGroupDomainEvent and publishes the corresponding integration event
/// </summary>
public class UserLeftGroupDomainEventHandler : INotificationHandler<UserLeftGroupDomainEvent>
{
    private readonly IEventPublisher _eventPublisher;
    
    public UserLeftGroupDomainEventHandler(IEventPublisher eventPublisher)
    {
        _eventPublisher = eventPublisher;
    }
    
    public async Task Handle(UserLeftGroupDomainEvent notification, CancellationToken cancellationToken)
    {
        // Map the domain event to an integration event
        var integrationEvent = new UserLeftGroupEvent(
            notification.GroupId,
            notification.UserId,
            notification.WasRemoved,
            notification.RemovedByUserId);
            
        // Publish the integration event to the message bus
        await _eventPublisher.PublishAsync(integrationEvent);
    }
}
