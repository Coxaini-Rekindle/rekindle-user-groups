namespace Rekindle.UserGroups.Contracts.GroupEvents;

using System;

/// <summary>
/// Event published when a group's information is updated
/// </summary>
public class GroupUpdatedEvent : Event
{
    /// <summary>
    /// The unique identifier of the group
    /// </summary>
    public Guid GroupId { get; }
    
    /// <summary>
    /// The new name of the group (if changed)
    /// </summary>
    public string Name { get; }
    
    /// <summary>
    /// The new description of the group (if changed)
    /// </summary>
    public string Description { get; }
    
    /// <summary>
    /// The user ID who performed the update
    /// </summary>
    public Guid UpdatedByUserId { get; }

    public GroupUpdatedEvent(Guid groupId, string name, string description, Guid updatedByUserId)
    {
        GroupId = groupId;
        Name = name;
        Description = description;
        UpdatedByUserId = updatedByUserId;
    }
}
