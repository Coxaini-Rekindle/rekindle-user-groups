using System.Net;
using Rekindle.Exceptions;

namespace Rekindle.UserGroups.Application.Authentication.Exceptions;

public class InvalidCredentialsException : AppException
{
    public InvalidCredentialsException() : base(
        "Invalid email or password",
        HttpStatusCode.Unauthorized,
        nameof(InvalidCredentialsException))
    {
    }
}