namespace Rekindle.UserGroups.Contracts.GroupEvents;

using System;

/// <summary>
/// Event published when a group invitation link is created
/// </summary>
public class GroupInvitationLinkCreatedEvent : Event
{
    /// <summary>
    /// The unique identifier of the invitation link
    /// </summary>
    public Guid InvitationLinkId { get; }
    
    /// <summary>
    /// The unique identifier of the group
    /// </summary>
    public Guid GroupId { get; }
    
    /// <summary>
    /// The user ID who created the invitation link
    /// </summary>
    public Guid CreatedByUserId { get; }
    
    /// <summary>
    /// The token string for the invitation link
    /// </summary>
    public string Token { get; }
    
    /// <summary>
    /// Maximum number of uses for this link
    /// </summary>
    public int MaxUses { get; }
    
    /// <summary>
    /// When the invitation link expires
    /// </summary>
    public DateTime ExpiresAt { get; }

    public GroupInvitationLinkCreatedEvent(
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
