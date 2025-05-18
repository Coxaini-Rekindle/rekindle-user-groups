using Rekindle.UserGroups.Domain.Entities.GroupUsers;

namespace Rekindle.UserGroups.Domain.Entities.Groups;

public class Group
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
    public ICollection<GroupUser> Members { get; private set; } = new List<GroupUser>();

    public static Group Create(string name, string description, DateTime createdAt)
    {
        return new Group
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            CreatedAt = createdAt
        };
    }
}