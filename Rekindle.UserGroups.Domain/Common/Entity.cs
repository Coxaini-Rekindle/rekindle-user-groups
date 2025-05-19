using System.Collections.ObjectModel;

namespace Rekindle.UserGroups.Domain.Common;

/// <summary>
/// Base class for all entities that need to track domain events
/// </summary>
public abstract class Entity
{
    private readonly List<DomainEvent> _domainEvents = new();
    
    /// <summary>
    /// Domain events occurred but not yet published
    /// </summary>
    public ReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    
    /// <summary>
    /// Add a domain event to this entity
    /// </summary>
    protected void AddDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
    
    /// <summary>
    /// Clear all domain events
    /// </summary>
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
