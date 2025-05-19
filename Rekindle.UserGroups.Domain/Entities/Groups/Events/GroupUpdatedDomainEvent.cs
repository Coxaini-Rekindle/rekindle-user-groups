using Rekindle.UserGroups.Domain.Common;

namespace Rekindle.UserGroups.Domain.Entities.Groups.Events;

/// <summary>
/// Domain event that occurs when a group's information is updated
/// </summary>
public class GroupUpdatedDomainEvent : DomainEvent
{
    public Guid GroupId { get; }
    public string Name { get; }
    public string Description { get; }
    public Guid UpdatedByUserId { get; }

    public GroupUpdatedDomainEvent(
        Guid groupId,
        string name,
        string description,
        Guid updatedByUserId)
    {
        GroupId = groupId;
        Name = name;
        Description = description;
        UpdatedByUserId = updatedByUserId;
    }
}
