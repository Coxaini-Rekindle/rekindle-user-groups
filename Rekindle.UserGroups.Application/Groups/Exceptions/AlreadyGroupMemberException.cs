using System.Net;
using Rekindle.Exceptions;

namespace Rekindle.UserGroups.Application.Groups.Exceptions;

public class AlreadyGroupMemberException : AppException
{
    public AlreadyGroupMemberException() : base(
        "User is already a member of this group",
        HttpStatusCode.Conflict,
        nameof(AlreadyGroupMemberException))
    {
    }
}
