using Rekindle.UserGroups.Contracts.Models;

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
    /// The user of the group
    /// </summary>
    public UserContractDto CreatedByUser { get; }
    
    /// <summary>
    /// When the group was created
    /// </summary>
    public DateTime CreatedAt { get; }

    public GroupCreatedEvent(Guid groupId, string name, string description, UserContractDto createdByUser,
        DateTime createdAt)
    {
        GroupId = groupId;
        Name = name;
        Description = description;
        CreatedByUser = createdByUser;
        CreatedAt = createdAt;
    }
}
