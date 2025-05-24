using System.Net;
using Rekindle.Exceptions;

namespace Rekindle.UserGroups.Application.Groups.Exceptions;

public class CannotRemoveGroupOwnerException : AppException
{
    public CannotRemoveGroupOwnerException() : base(
        "Cannot remove the group owner. Transfer ownership first or leave the group.",
        HttpStatusCode.BadRequest,
        nameof(CannotRemoveGroupOwnerException))
    {
    }
}
