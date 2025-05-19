namespace Rekindle.UserGroups.Contracts.UserEvents;

using System;

/// <summary>
/// Event published when a new user is registered in the system
/// </summary>
public class UserCreatedEvent : Event
{
    /// <summary>
    /// The unique identifier of the user
    /// </summary>
    public Guid UserId { get; }
    
    /// <summary>
    /// The user's email address
    /// </summary>
    public string Email { get; }
    
    /// <summary>
    /// The user's display name
    /// </summary>
    public string Name { get; }
    
    /// <summary>
    /// The user's username
    /// </summary>
    public string Username { get; }

    public UserCreatedEvent(Guid userId, string email, string name, string username)
    {
        UserId = userId;
        Email = email;
        Name = name;
        Username = username;
    }
}
