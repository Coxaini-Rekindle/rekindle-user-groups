using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using Rekindle.UserGroups.Application.Authentication.Interfaces;
using Rekindle.UserGroups.Application.Authentication.Models;

namespace Rekindle.UserGroups.Infrastructure.Security.Jwt;

public class RefreshTokenGenerator : IRefreshTokenGenerator
{
    private readonly IOptions<JwtSettings> _jwtSettings;

    public RefreshTokenGenerator(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings;
    }

    public RefreshToken GenerateRefreshToken()
    {
        var refreshToken = new RefreshToken
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            ExpiryTime = DateTime.UtcNow.AddMinutes(_jwtSettings.Value.RefreshTokenExpiryMinutes)
        };

        return refreshToken;
    }
}