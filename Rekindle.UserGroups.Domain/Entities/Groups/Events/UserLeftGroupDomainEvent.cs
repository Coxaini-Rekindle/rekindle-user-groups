using Rekindle.UserGroups.Domain.Common;

namespace Rekindle.UserGroups.Domain.Entities.Groups.Events;

/// <summary>
/// Domain event that occurs when a user leaves or is removed from a group
/// </summary>
public class UserLeftGroupDomainEvent : DomainEvent
{
    public Guid GroupId { get; }
    public Guid UserId { get; }
    public bool WasRemoved { get; }
    public Guid? RemovedByUserId { get; }

    public UserLeftGroupDomainEvent(
        Guid groupId,
        Guid userId,
        bool wasRemoved = false,
        Guid? removedByUserId = null)
    {
        GroupId = groupId;
        UserId = userId;
        WasRemoved = wasRemoved;
        RemovedByUserId = removedByUserId;
    }
}
