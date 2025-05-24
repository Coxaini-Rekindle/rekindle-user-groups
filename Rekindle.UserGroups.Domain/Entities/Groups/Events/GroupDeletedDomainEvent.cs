using Rekindle.UserGroups.Domain.Common;

namespace Rekindle.UserGroups.Domain.Entities.Groups.Events;

/// <summary>
/// Domain event that occurs when a group is deleted
/// </summary>
public class GroupDeletedDomainEvent : DomainEvent
{
    public Guid GroupId { get; }
    public string Name { get; }
    public string Description { get; }
    public Guid DeletedByUserId { get; }
    public DateTime DeletedAt { get; }

    public GroupDeletedDomainEvent(
        Guid groupId,
        string name,
        string description,
        Guid deletedByUserId,
        DateTime deletedAt)
    {
        GroupId = groupId;
        Name = name;
        Description = description;
        DeletedByUserId = deletedByUserId;
        DeletedAt = deletedAt;
    }
}
