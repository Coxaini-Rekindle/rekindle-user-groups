using System.Net;
using Rekindle.Exceptions;

namespace Rekindle.UserGroups.Application.Groups.Exceptions;

public class NotGroupOwnerException : AppException
{
    public NotGroupOwnerException() : base(
        "You are not the owner of this group",
        HttpStatusCode.Forbidden,
        nameof(NotGroupOwnerException))
    {
    }
}
