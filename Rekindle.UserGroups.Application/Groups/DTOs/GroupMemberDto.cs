using Rekindle.UserGroups.Domain.Entities.GroupUsers;
using Rekindle.UserGroups.Domain.Entities.GroupUsers.Enumerations;

namespace Rekindle.UserGroups.Application.Groups.DTOs;

public class GroupMemberDto
{
    public Guid UserId { get; set; }
    public string Name { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public GroupUserRole Role { get; set; }
    public DateTime JoinedAt { get; set; }
    
    public static GroupMemberDto FromGroupUser(GroupUser groupUser)
    {
        return new GroupMemberDto
        {
            UserId = groupUser.UserId,
            Name = groupUser.User.Name,
            Username = groupUser.User.Username,
            Email = groupUser.User.Email,
            Role = groupUser.Role,
            JoinedAt = groupUser.JoinedAt
        };
    }
}
