namespace Rekindle.UserGroups.Application.Authentication.Models;

public record RefreshToken(string Token = "", DateTime ExpiryTime = default);