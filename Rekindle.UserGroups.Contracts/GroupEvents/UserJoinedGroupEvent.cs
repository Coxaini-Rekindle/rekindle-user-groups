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
    ///  The username of the user who joined
    /// </summary>
    public string UserName { get; }

    /// <summary>
    /// The name of the user who joined
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// The avatar file ID of the user who joined, if any
    /// </summary>
    public Guid? AvatarFileId { get; }
    
    /// <summary>
    /// The role assigned to the user in the group
    /// </summary>
    public string Role { get; }
    
    /// <summary>
    /// When the user joined the group
    /// </summary>
    public DateTime JoinedAt { get; }

    public UserJoinedGroupEvent(Guid groupId,
        Guid userId,
        string userName,
        string name,
        Guid? avatarFileId,
        string role,
        DateTime joinedAt)
    {
        GroupId = groupId;
        UserId = userId;
        UserName = userName;
        Name = name;
        AvatarFileId = avatarFileId;
        Role = role;
        JoinedAt = joinedAt;
    }
}
