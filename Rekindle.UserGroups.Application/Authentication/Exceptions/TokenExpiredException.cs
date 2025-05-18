using System.Net;
using Rekindle.Exceptions;

namespace Rekindle.UserGroups.Application.Authentication.Exceptions;

public class TokenExpiredException : AppException
{
    public TokenExpiredException() : base(
        "The token has expired",
        HttpStatusCode.Unauthorized,
        nameof(TokenExpiredException))
    {
    }
}