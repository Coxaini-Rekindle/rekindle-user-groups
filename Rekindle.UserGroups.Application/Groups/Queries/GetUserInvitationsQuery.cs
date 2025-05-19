using MediatR;
using Microsoft.EntityFrameworkCore;
using Rekindle.UserGroups.Application.Groups.DTOs;
using Rekindle.UserGroups.DataAccess;
using Rekindle.UserGroups.Domain.Entities.GroupInvites.Enumerations;

namespace Rekindle.UserGroups.Application.Groups.Queries;

public record GetUserInvitationsQuery(Guid UserId) : IRequest<List<GroupInviteDto>>;

public class GetUserInvitationsQueryHandler : IRequestHandler<GetUserInvitationsQuery, List<GroupInviteDto>>
{
    private readonly UserGroupsDbContext _dbContext;

    public GetUserInvitationsQueryHandler(UserGroupsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<GroupInviteDto>> Handle(GetUserInvitationsQuery request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user == null)
        {
            throw new Authentication.Exceptions.UserNotFoundException();
        }

        // Get invites either directly by user ID or by email
        var invites = await _dbContext.GroupInvites
            .Where(gi => 
                (gi.InvitedUserId == request.UserId || gi.Email == user.Email) && 
                gi.Status == InvitationStatus.Pending &&
                gi.ExpiresAt > DateTime.UtcNow)
            .Include(gi => gi.Group)
            .Include(gi => gi.InvitedBy)
            .OrderByDescending(gi => gi.CreatedAt)
            .ToListAsync(cancellationToken);

        return invites
            .Select(GroupInviteDto.FromGroupInvite)
            .ToList();
    }
}
