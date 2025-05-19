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
    public GroupUserRole Role { get; }
    public DateTime JoinedAt { get; }

    public UserJoinedGroupDomainEvent(
        Guid groupId,
        Guid userId,
        GroupUserRole role,
        DateTime joinedAt)
    {
        GroupId = groupId;
        UserId = userId;
        Role = role;
        JoinedAt = joinedAt;
    }
}
