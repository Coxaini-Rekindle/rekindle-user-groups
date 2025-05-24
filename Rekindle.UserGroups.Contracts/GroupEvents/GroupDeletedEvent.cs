namespace Rekindle.UserGroups.Contracts.GroupEvents;

using System;

/// <summary>
/// Event published when a group is deleted
/// </summary>
public class GroupDeletedEvent : Event
{
    /// <summary>
    /// The unique identifier of the deleted group
    /// </summary>
    public Guid GroupId { get; }
    
    /// <summary>
    /// The name of the deleted group
    /// </summary>
    public string Name { get; }
    
    /// <summary>
    /// The description of the deleted group
    /// </summary>
    public string Description { get; }
    
    /// <summary>
    /// The user ID who deleted the group
    /// </summary>
    public Guid DeletedByUserId { get; }
    
    /// <summary>
    /// When the group was deleted
    /// </summary>
    public DateTime DeletedAt { get; }

    public GroupDeletedEvent(Guid groupId, string name, string description, Guid deletedByUserId, DateTime deletedAt)
    {
        GroupId = groupId;
        Name = name;
        Description = description;
        DeletedByUserId = deletedByUserId;
        DeletedAt = deletedAt;
    }
}
