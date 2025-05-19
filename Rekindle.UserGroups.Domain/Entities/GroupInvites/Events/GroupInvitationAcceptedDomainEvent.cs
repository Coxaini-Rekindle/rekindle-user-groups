using Rekindle.UserGroups.Domain.Common;

namespace Rekindle.UserGroups.Domain.Entities.GroupInvites.Events;

/// <summary>
/// Domain event that occurs when a group invitation is accepted
/// </summary>
public class GroupInvitationAcceptedDomainEvent : DomainEvent
{
    public Guid InvitationId { get; }
    public Guid GroupId { get; }
    public Guid UserId { get; }

    public GroupInvitationAcceptedDomainEvent(
        Guid invitationId,
        Guid groupId,
        Guid userId)
    {
        InvitationId = invitationId;
        GroupId = groupId;
        UserId = userId;
    }
}
