using MediatR;
using Microsoft.EntityFrameworkCore;
using Rekindle.UserGroups.Application.Groups.DTOs;
using Rekindle.UserGroups.Application.Groups.Exceptions;
using Rekindle.UserGroups.DataAccess;

namespace Rekindle.UserGroups.Application.Groups.Queries;

/// <summary>
/// Query to get group information by public invitation token
/// </summary>
public record GetGroupInfoByTokenQuery(string Token, Guid? CurrentUserId = null) : IRequest<GroupDto>;

public class GetGroupInfoByTokenQueryHandler : IRequestHandler<GetGroupInfoByTokenQuery, GroupDto>
{
    private readonly UserGroupsDbContext _dbContext;

    public GetGroupInfoByTokenQueryHandler(UserGroupsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GroupDto> Handle(GetGroupInfoByTokenQuery request, CancellationToken cancellationToken)
    {
        // Find the invitation link
        var invitationLink = await _dbContext.InvitationLinks
            .Include(il => il.Group)
                .ThenInclude(g => g.Members)
            .FirstOrDefaultAsync(il => il.Token == request.Token, cancellationToken);

        if (invitationLink == null)
        {
            throw new InviteNotFoundException();
        }

        // Check if invitation link is still valid
        if (!invitationLink.IsValid())
        {
            throw new InviteNotFoundException();
        }

        return GroupDto.FromGroup(invitationLink.Group, request.CurrentUserId);
    }
}
