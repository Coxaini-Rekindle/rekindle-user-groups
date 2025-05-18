using Rekindle.UserGroups.Domain.Entities.GroupUsers.Enumerations;

namespace Rekindle.UserGroups.Domain.Entities.GroupUsers;

public class GroupUser
{
    public Guid UserId { get; private set; }
    public Guid GroupId { get; private set; }
    public GroupUserRole Role { get; private set; }
}