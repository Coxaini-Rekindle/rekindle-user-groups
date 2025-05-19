namespace Rekindle.UserGroups.Contracts.GroupEvents;

using System;

/// <summary>
/// Event published when a user leaves or is removed from a group
/// </summary>
public class UserLeftGroupEvent : Event
{
    /// <summary>
    /// The unique identifier of the group
    /// </summary>
    public Guid GroupId { get; }
    
    /// <summary>
    /// The unique identifier of the user who left/was removed
    /// </summary>
    public Guid UserId { get; }
    
    /// <summary>
    /// Whether the user was removed by another user or left voluntarily
    /// </summary>
    public bool WasRemoved { get; }
    
    /// <summary>
    /// The user ID who removed this user (null if the user left voluntarily)
    /// </summary>
    public Guid? RemovedByUserId { get; }

    public UserLeftGroupEvent(Guid groupId, Guid userId, bool wasRemoved = false, Guid? removedByUserId = null)
    {
        GroupId = groupId;
        UserId = userId;
        WasRemoved = wasRemoved;
        RemovedByUserId = removedByUserId;
    }
}
