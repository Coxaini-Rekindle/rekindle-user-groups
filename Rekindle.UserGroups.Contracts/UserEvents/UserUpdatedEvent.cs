namespace Rekindle.UserGroups.Contracts.UserEvents;

using System;

/// <summary>
/// Event published when a user's profile information is updated
/// </summary>
public class UserUpdatedEvent : Event
{
    /// <summary>
    /// The unique identifier of the user
    /// </summary>
    public Guid UserId { get; }
    
    /// <summary>
    /// The user's email address (if changed)
    /// </summary>
    public string? Email { get; }
    
    /// <summary>
    /// The user's display name (if changed)
    /// </summary>
    public string? Name { get; }
    
    /// <summary>
    /// The user's username (if changed)
    /// </summary>
    public string? Username { get; }

    public UserUpdatedEvent(Guid userId, string? email = null, string? name = null, string? username = null)
    {
        UserId = userId;
        Email = email;
        Name = name;
        Username = username;
    }
}
