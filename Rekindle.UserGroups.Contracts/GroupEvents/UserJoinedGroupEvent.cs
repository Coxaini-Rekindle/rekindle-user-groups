namespace Rekindle.UserGroups.Contracts.GroupEvents;

using System;

/// <summary>
/// Event published when a user joins a group
/// </summary>
public class UserJoinedGroupEvent : Event
{
    /// <summary>
    /// The unique identifier of the group
    /// </summary>
    public Guid GroupId { get; }
    
    /// <summary>
    /// The unique identifier of the user who joined
    /// </summary>
    public Guid UserId { get; }
    
    /// <summary>
    /// The role assigned to the user in the group
    /// </summary>
    public string Role { get; }
    
    /// <summary>
    /// When the user joined the group
    /// </summary>
    public DateTime JoinedAt { get; }

    public UserJoinedGroupEvent(Guid groupId, Guid userId, string role, DateTime joinedAt)
    {
        GroupId = groupId;
        UserId = userId;
        Role = role;
        JoinedAt = joinedAt;
    }
}
