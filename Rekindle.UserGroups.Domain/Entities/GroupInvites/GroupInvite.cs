using Rekindle.UserGroups.Domain.Common;
using Rekindle.UserGroups.Domain.Entities.GroupInvites.Enumerations;
using Rekindle.UserGroups.Domain.Entities.GroupInvites.Events;
using Rekindle.UserGroups.Domain.Entities.Groups;
using Rekindle.UserGroups.Domain.Entities.Users;

namespace Rekindle.UserGroups.Domain.Entities.GroupInvites;

public class GroupInvite : Entity
{
    public Guid Id { get; private set; }
    public Guid GroupId { get; private set; }
    public Guid InvitedByUserId { get; private set; }
    public Guid? InvitedUserId { get; private set; }
    public string? Email { get; private set; }
    public InvitationStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime ExpiresAt { get; private set; }

    // Navigation properties
    public Group Group { get; private set; } = null!;
    public User InvitedBy { get; private set; } = null!;
    public User? InvitedUser { get; private set; }

    private GroupInvite()
    {
    }

    public static GroupInvite Create(
        Guid groupId,
        Guid invitedByUserId,
        Guid? invitedUserId = null,
        string? email = null,
        int expirationDays = 7)
    {        if (invitedUserId == null && string.IsNullOrEmpty(email))
            throw new ArgumentException("Either invitedUserId or email must be provided");

        var groupInvite = new GroupInvite
        {
            Id = Guid.NewGuid(),
            GroupId = groupId,
            InvitedByUserId = invitedByUserId,
            InvitedUserId = invitedUserId,
            Email = email,
            Status = InvitationStatus.Pending,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(expirationDays)
        };
        
        // Add domain event for group invitation created
        groupInvite.AddDomainEvent(new GroupInvitationCreatedDomainEvent(
            groupInvite.Id,
            groupInvite.GroupId,
            groupInvite.InvitedByUserId,
            groupInvite.InvitedUserId,
            groupInvite.Email,
            groupInvite.ExpiresAt
        ));
        
        return groupInvite;
    }    public bool Accept()
    {
        if (Status != InvitationStatus.Pending || IsExpired()) return false;

        Status = InvitationStatus.Accepted;
        
        // Add domain event for group invitation accepted
        if (InvitedUserId.HasValue)
        {
            AddDomainEvent(new GroupInvitationAcceptedDomainEvent(
                Id,
                GroupId,
                InvitedUserId.Value
            ));
        }
        
        return true;
    }

    public bool Decline()
    {
        if (Status != InvitationStatus.Pending || IsExpired()) return false;

        Status = InvitationStatus.Declined;
        return true;
    }

    public bool IsExpired()
    {
        if (DateTime.UtcNow > ExpiresAt)
        {
            Status = InvitationStatus.Expired;
            return true;
        }

        return false;
    }

    public void AssignToUser(Guid userId)
    {
        if (InvitedUserId == null && !string.IsNullOrEmpty(Email)) InvitedUserId = userId;
    }
}