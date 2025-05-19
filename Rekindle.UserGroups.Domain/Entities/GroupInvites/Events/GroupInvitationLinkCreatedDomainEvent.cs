using Rekindle.UserGroups.Domain.Common;

namespace Rekindle.UserGroups.Domain.Entities.GroupInvites.Events;

/// <summary>
/// Domain event that occurs when a group invitation link is created
/// </summary>
public class GroupInvitationLinkCreatedDomainEvent : DomainEvent
{
    public Guid InvitationLinkId { get; }
    public Guid GroupId { get; }
    public Guid CreatedByUserId { get; }
    public string Token { get; }
    public int MaxUses { get; }
    public DateTime ExpiresAt { get; }

    public GroupInvitationLinkCreatedDomainEvent(
        Guid invitationLinkId,
        Guid groupId,
        Guid createdByUserId,
        string token,
        int maxUses,
        DateTime expiresAt)
    {
        InvitationLinkId = invitationLinkId;
        GroupId = groupId;
        CreatedByUserId = createdByUserId;
        Token = token;
        MaxUses = maxUses;
        ExpiresAt = expiresAt;
    }
}
