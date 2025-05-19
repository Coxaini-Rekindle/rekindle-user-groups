using Rekindle.UserGroups.Domain.Entities.GroupUsers;
using Rekindle.UserGroups.Domain.Entities.GroupUsers.Enumerations;
using Rekindle.UserGroups.Domain.Entities.Users;

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

    public GroupUser AddMember(User user, GroupUserRole role = GroupUserRole.Member)
    {
        var groupUser = GroupUser.Create(user.Id, Id, role);
        Members.Add(groupUser);
        return groupUser;
    }

    public GroupUser? GetMember(Guid userId)
    {
        return Members.FirstOrDefault(m => m.UserId == userId);
    }

    public bool RemoveMember(Guid userId)
    {
        var member = GetMember(userId);
        if (member == null) return false;

        return Members.Remove(member);
    }

    public void Update(string name, string description)
    {
        Name = name;
        Description = description;
    }

    private Group()
    {
    }
}

