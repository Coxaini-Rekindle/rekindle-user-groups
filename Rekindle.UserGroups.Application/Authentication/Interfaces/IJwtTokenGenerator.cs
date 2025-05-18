using System.Security.Claims;
using Rekindle.UserGroups.Application.Authentication.Models;

namespace Rekindle.UserGroups.Application.Authentication.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(UserClaims userClaims);
    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}