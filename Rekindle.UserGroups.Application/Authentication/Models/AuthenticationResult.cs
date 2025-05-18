namespace Rekindle.UserGroups.Application.Authentication.Models;

public record AuthenticationResult(
    string AccessToken,
    string RefreshToken,
    DateTime RefreshTokenExpiryTime
);