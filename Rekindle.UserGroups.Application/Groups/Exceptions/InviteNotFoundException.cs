using System.Net;
using Rekindle.Exceptions;

namespace Rekindle.UserGroups.Application.Groups.Exceptions;

public class InviteNotFoundException : AppException
{
    public InviteNotFoundException() : base(
        "Invite not found or has expired",
        HttpStatusCode.NotFound,
        nameof(InviteNotFoundException))
    {
    }
}
