using MediatR;
using Microsoft.EntityFrameworkCore;
using Rekindle.UserGroups.Application.Groups.DTOs;
using Rekindle.UserGroups.Application.Groups.Exceptions;
using Rekindle.UserGroups.DataAccess;
using Rekindle.UserGroups.Domain.Entities.GroupInvites.Enumerations;

namespace Rekindle.UserGroups.Application.Groups.Queries;

/// <summary>
/// Query to get group information by personal invitation ID
/// </summary>
public record GetGroupInfoByInviteIdQuery(Guid InviteId, Guid? CurrentUserId = null) : IRequest<GroupDto>;

public class GetGroupInfoByInviteIdQueryHandler : IRequestHandler<GetGroupInfoByInviteIdQuery, GroupDto>
{
    private readonly UserGroupsDbContext _dbContext;

    public GetGroupInfoByInviteIdQueryHandler(UserGroupsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GroupDto> Handle(GetGroupInfoByInviteIdQuery request, CancellationToken cancellationToken)
    {
        // Find the invitation
        var invitation = await _dbContext.GroupInvites
            .Include(gi => gi.Group)
            .ThenInclude(g => g.Members)
            .FirstOrDefaultAsync(gi => gi.Id == request.InviteId, cancellationToken);

        if (invitation == null) throw new InviteNotFoundException();

        // Check if invitation is still valid
        if (invitation.Status != InvitationStatus.Pending || invitation.IsExpired())
            throw new InviteNotFoundException();

        // If a current user is provided, verify they can access this invitation
        if (request.CurrentUserId.HasValue)
        {
            var currentUser = await _dbContext.Users.FindAsync([request.CurrentUserId.Value], cancellationToken);
            if (currentUser != null)
            {
                // Check if invitation is for this user (either by user ID or email)
                var canAccess = invitation.InvitedUserId == request.CurrentUserId.Value ||
                                (!string.IsNullOrEmpty(invitation.Email) && invitation.Email == currentUser.Email);

                if (!canAccess) throw new InviteNotFoundException();
            }
        }

        return GroupDto.FromGroup(invitation.Group, request.CurrentUserId);
    }
}