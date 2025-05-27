using Rekindle.UserGroups.Domain.Common;
using Rekindle.UserGroups.Domain.Entities.GroupUsers.Enumerations;

namespace Rekindle.UserGroups.Domain.Entities.Groups.Events;

/// <summary>
/// Domain event that occurs when a user joins a group
/// </summary>
public class UserJoinedGroupDomainEvent : DomainEvent
{
    public Guid GroupId { get; }
    public Guid UserId { get; }
    public string UserName { get; }
    public string Name { get; }
    public Guid? AvatarFileId { get; }
    public GroupUserRole Role { get; }
    public DateTime JoinedAt { get; }

    public UserJoinedGroupDomainEvent(
        Guid groupId,
        Guid userId,
        string userName,
        string name,
        Guid? avatarFileId,
        GroupUserRole role,
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
