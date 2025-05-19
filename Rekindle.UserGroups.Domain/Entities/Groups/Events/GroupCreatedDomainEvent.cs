using Rekindle.UserGroups.Domain.Common;

namespace Rekindle.UserGroups.Domain.Entities.Groups.Events;

/// <summary>
/// Domain event that occurs when a new group is created
/// </summary>
public class GroupCreatedDomainEvent : DomainEvent
{
    public Guid GroupId { get; }
    public string Name { get; }
    public string Description { get; }
    public Guid CreatedByUserId { get; }
    public DateTime CreatedAt { get; }

    public GroupCreatedDomainEvent(
        Guid groupId, 
        string name, 
        string description, 
        Guid createdByUserId,
        DateTime createdAt)
    {
        GroupId = groupId;
        Name = name;
        Description = description;
        CreatedByUserId = createdByUserId;
        CreatedAt = createdAt;
    }
}
