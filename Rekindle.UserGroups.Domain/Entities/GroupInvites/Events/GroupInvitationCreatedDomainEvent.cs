using Rekindle.UserGroups.Domain.Common;

namespace Rekindle.UserGroups.Domain.Entities.GroupInvites.Events;

/// <summary>
/// Domain event that occurs when a group invitation is created
/// </summary>
public class GroupInvitationCreatedDomainEvent : DomainEvent
{
    public Guid InvitationId { get; }
    public Guid GroupId { get; }
    public Guid InvitedByUserId { get; }
    public Guid? InvitedUserId { get; }
    public string? Email { get; }
    public DateTime ExpiresAt { get; }

    public GroupInvitationCreatedDomainEvent(
        Guid invitationId,
        Guid groupId,
        Guid invitedByUserId,
        Guid? invitedUserId,
        string? email,
        DateTime expiresAt)
    {
        InvitationId = invitationId;
        GroupId = groupId;
        InvitedByUserId = invitedByUserId;
        InvitedUserId = invitedUserId;
        Email = email;
        ExpiresAt = expiresAt;
    }
}
