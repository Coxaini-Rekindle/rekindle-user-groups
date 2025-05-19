using MediatR;
using Rekindle.UserGroups.Application.Common.Interfaces;
using Rekindle.UserGroups.Domain.Common;

namespace Rekindle.UserGroups.Infrastructure.DomainEvents;

/// <summary>
/// Service for dispatching domain events to their respective handlers
/// </summary>
public class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IPublisher _mediator;

    public DomainEventDispatcher(IPublisher mediator)
    {
        _mediator = mediator;
    }

    public async Task DispatchEventsAsync(IEnumerable<DomainEvent> domainEvents)
    {
        foreach (var domainEvent in domainEvents) await _mediator.Publish(domainEvent);
    }
}