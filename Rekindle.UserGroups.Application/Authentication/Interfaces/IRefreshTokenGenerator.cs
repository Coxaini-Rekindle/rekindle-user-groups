using Rekindle.UserGroups.Application.Authentication.Models;

namespace Rekindle.UserGroups.Application.Authentication.Interfaces;

public interface IRefreshTokenGenerator
{
    public RefreshToken GenerateRefreshToken();
}