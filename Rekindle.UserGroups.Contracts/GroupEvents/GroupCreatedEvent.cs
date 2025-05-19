namespace Rekindle.UserGroups.Contracts.GroupEvents;

using System;

/// <summary>
/// Event published when a new group is created
/// </summary>
public class GroupCreatedEvent : Event
{
    /// <summary>
    /// The unique identifier of the group
    /// </summary>
    public Guid GroupId { get; }
    
    /// <summary>
    /// The name of the group
    /// </summary>
    public string Name { get; }
    
    /// <summary>
    /// The description of the group
    /// </summary>
    public string Description { get; }
    
    /// <summary>
    /// The user ID of the group creator/owner
    /// </summary>
    public Guid CreatedByUserId { get; }
    
    /// <summary>
    /// When the group was created
    /// </summary>
    public DateTime CreatedAt { get; }

    public GroupCreatedEvent(Guid groupId, string name, string description, Guid createdByUserId, DateTime createdAt)
    {
        GroupId = groupId;
        Name = name;
        Description = description;
        CreatedByUserId = createdByUserId;
        CreatedAt = createdAt;
    }
}
