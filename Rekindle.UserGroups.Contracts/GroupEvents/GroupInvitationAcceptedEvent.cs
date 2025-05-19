namespace Rekindle.UserGroups.Contracts.GroupEvents;

using System;

/// <summary>
/// Event published when a group invitation is accepted by a user
/// </summary>
public class GroupInvitationAcceptedEvent : Event
{
    /// <summary>
    /// The unique identifier of the invitation
    /// </summary>
    public Guid InvitationId { get; }
    
    /// <summary>
    /// The unique identifier of the group
    /// </summary>
    public Guid GroupId { get; }
    
    /// <summary>
    /// The user ID who accepted the invitation
    /// </summary>
    public Guid UserId { get; }

    public GroupInvitationAcceptedEvent(Guid invitationId, Guid groupId, Guid userId)
    {
        InvitationId = invitationId;
        GroupId = groupId;
        UserId = userId;
    }
}
