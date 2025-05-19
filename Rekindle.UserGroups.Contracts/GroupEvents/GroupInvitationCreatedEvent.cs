namespace Rekindle.UserGroups.Contracts.GroupEvents;

using System;

/// <summary>
/// Event published when a group invitation is created
/// </summary>
public class GroupInvitationCreatedEvent : Event
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
    /// The user ID who sent the invitation
    /// </summary>
    public Guid InvitedByUserId { get; }
    
    /// <summary>
    /// The user ID who was invited (may be null if invited by email only)
    /// </summary>
    public Guid? InvitedUserId { get; }
    
    /// <summary>
    /// The email address that was invited (may be null if a specific user was invited)
    /// </summary>
    public string? Email { get; }
    
    /// <summary>
    /// When the invitation expires
    /// </summary>
    public DateTime ExpiresAt { get; }

    public GroupInvitationCreatedEvent(
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
