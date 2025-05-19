using Rekindle.UserGroups.Domain.Entities.GroupInvites;
using Rekindle.UserGroups.Domain.Entities.GroupInvites.Enumerations;

namespace Rekindle.UserGroups.Application.Groups.DTOs;

public class GroupInviteDto
{
    public Guid Id { get; set; }
    public Guid GroupId { get; set; }
    public string GroupName { get; set; } = null!;
    public string InvitedByUsername { get; set; } = null!;
    public string? InvitedUsername { get; set; }
    public string? Email { get; set; }
    public InvitationStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    
    public static GroupInviteDto FromGroupInvite(GroupInvite invite)
    {
        return new GroupInviteDto
        {
            Id = invite.Id,
            GroupId = invite.GroupId,
            GroupName = invite.Group.Name,
            InvitedByUsername = invite.InvitedBy.Username,
            InvitedUsername = invite.InvitedUser?.Username,
            Email = invite.Email,
            Status = invite.Status,
            CreatedAt = invite.CreatedAt,
            ExpiresAt = invite.ExpiresAt
        };
    }
}
