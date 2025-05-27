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
    public CreatorUser Creator { get; }
    public DateTime CreatedAt { get; }

    public GroupCreatedDomainEvent(
        Guid groupId,
        string name,
        string description,
        CreatorUser creator,
        DateTime createdAt)
    {
        GroupId = groupId;
        Name = name;
        Description = description;
        Creator = creator;
        CreatedAt = createdAt;
    }
}

public class CreatorUser
{
    public Guid Id { get; }
    public string Name { get; }
    public string Username { get; }
    public Guid? AvatarFileId { get; }

    public CreatorUser(Guid id, string name, string username, Guid? avatarFileId)
    {
        Id = id;
        Name = name;
        Username = username;
        AvatarFileId = avatarFileId;
    }
}