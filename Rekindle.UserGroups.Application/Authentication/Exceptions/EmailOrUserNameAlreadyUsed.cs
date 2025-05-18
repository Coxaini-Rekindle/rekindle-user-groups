using System.Net;
using Rekindle.Exceptions;

namespace Rekindle.UserGroups.Application.Authentication.Exceptions;

public class EmailOrUserNameAlreadyUsed : AppException
{
    public EmailOrUserNameAlreadyUsed() : base(
        "Email or username already used",
        HttpStatusCode.Conflict,
        nameof(EmailOrUserNameAlreadyUsed))
    {
    }
}