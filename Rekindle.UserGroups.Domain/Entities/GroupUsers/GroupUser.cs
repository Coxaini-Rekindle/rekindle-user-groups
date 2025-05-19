using Rekindle.UserGroups.Domain.Entities.GroupUsers.Enumerations;
using Rekindle.UserGroups.Domain.Entities.Groups;
using Rekindle.UserGroups.Domain.Entities.Users;

namespace Rekindle.UserGroups.Domain.Entities.GroupUsers;

public class GroupUser
{
    public Guid UserId { get; private set; }
    public Guid GroupId { get; private set; }
    public GroupUserRole Role { get; private set; }
    public DateTime JoinedAt { get; private set; }

    // Navigation properties
    public User User { get; private set; } = null!;
    public Group Group { get; private set; } = null!;

    private GroupUser()
    {
    }

    public static GroupUser Create(Guid userId, Guid groupId, GroupUserRole role)
    {
        return new GroupUser
        {
            UserId = userId,
            GroupId = groupId,
            Role = role,
            JoinedAt = DateTime.UtcNow
        };
    }

    public void UpdateRole(GroupUserRole newRole)
    {
        Role = newRole;
    }
}

