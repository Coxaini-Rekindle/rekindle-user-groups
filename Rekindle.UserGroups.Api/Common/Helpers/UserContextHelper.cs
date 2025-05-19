using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Rekindle.UserGroups.Api.Common.Helpers;

public static class UserContextHelper
{
    public static Guid? GetCurrentUserId(ClaimsPrincipal? user)
    {
        if (user == null) return null;

        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                     user.FindFirst("sub")?.Value ??
                     user.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ??
                     user.FindFirst(JwtRegisteredClaimNames.NameId)?.Value;

        if (string.IsNullOrEmpty(userId)) return null;

        return Guid.Parse(userId);
    }
}