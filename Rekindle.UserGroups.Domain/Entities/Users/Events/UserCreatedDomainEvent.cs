using Rekindle.UserGroups.Domain.Common;

namespace Rekindle.UserGroups.Domain.Entities.Users.Events;

public class UserCreatedDomainEvent : DomainEvent
{
    public Guid UserId { get; }
    public string Email { get; }
    public string Username { get; }
    public string Name { get; }

    public UserCreatedDomainEvent(Guid userId, string email, string username, string name)
    {
        UserId = userId;
        Email = email;
        Username = username;
        Name = name;
    }
}