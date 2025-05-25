using Rekindle.UserGroups.Domain.Common;
using Rekindle.UserGroups.Domain.Entities.Users.Events;

namespace Rekindle.UserGroups.Domain.Entities.Users;

public class User : Entity
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string Username { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;
    public string? RefreshToken { get; private set; }
    public DateTime? RefreshTokenExpiryTime { get; private set; }
    public Guid? AvatarFileId { get; private set; }

    public static User Create(string name, string username, string email, string passwordHash)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = name,
            Username = username,
            Email = email,
            PasswordHash = passwordHash
        };

        user.AddDomainEvent(new UserCreatedDomainEvent(user.Id, user.Email, user.Username, user.Name));

        return user;
    }

    public void SetRefreshToken(string token, DateTime expiryTime)
    {
        RefreshToken = token;
        RefreshTokenExpiryTime = expiryTime;
    }

    public bool IsRefreshTokenValid(string token)
    {
        return RefreshToken == token && RefreshTokenExpiryTime > DateTime.UtcNow;
    }

    public void UpdateName(string name)
    {
        AddDomainEvent(new UserNameChangedDomainEvent(Id, Name, name));
        Name = name;
    }

    public void SetAvatar(Guid? avatarFileId)
    {
        AddDomainEvent(new UserAvatarChangedDomainEvent(Id, AvatarFileId, avatarFileId));
        AvatarFileId = avatarFileId;
    }

    private User()
    {
    }
}