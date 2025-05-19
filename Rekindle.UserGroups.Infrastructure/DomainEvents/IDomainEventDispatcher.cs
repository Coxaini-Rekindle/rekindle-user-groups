using Rekindle.UserGroups.Domain.Common;

namespace Rekindle.UserGroups.Infrastructure.DomainEvents;

/// <summary>
/// Interface for dispatching domain events
/// </summary>
public interface IDomainEventDispatcher
{
    /// <summary>
    /// Dispatch domain events to their handlers
    /// </summary>
    /// <param name="domainEvents">The domain events to dispatch</param>
    Task DispatchEventsAsync(IEnumerable<DomainEvent> domainEvents);
}
