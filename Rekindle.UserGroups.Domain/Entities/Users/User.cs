namespace Rekindle.UserGroups.Domain.Entities.Users;

public class User
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string Username { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;
    public string? RefreshToken { get; private set; }
    public DateTime? RefreshTokenExpiryTime { get; private set; }

    public static User Create(string name, string username, string email, string passwordHash)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            Name = name,
            Username = username,
            Email = email,
            PasswordHash = passwordHash
        };
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

    private User()
    {
    }
}