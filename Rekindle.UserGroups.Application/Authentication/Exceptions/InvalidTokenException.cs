using System.Net;
using Rekindle.Exceptions;

namespace Rekindle.UserGroups.Application.Authentication.Exceptions;

public class InvalidTokenException : AppException
{
    public InvalidTokenException() : base(
        "Invalid token provided",
        HttpStatusCode.Unauthorized,
        nameof(InvalidTokenException))
    {
    }
}