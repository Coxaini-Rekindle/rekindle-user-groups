using System.Net;
using Rekindle.Exceptions;

namespace Rekindle.UserGroups.Application.Groups.Exceptions;

public class NotGroupMemberException : AppException
{
    public NotGroupMemberException() : base(
        "You are not a member of this group",
        HttpStatusCode.Forbidden,
        nameof(NotGroupMemberException))
    {
    }
}
