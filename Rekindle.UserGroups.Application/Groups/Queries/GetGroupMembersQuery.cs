using MediatR;
using Microsoft.EntityFrameworkCore;
using Rekindle.UserGroups.Application.Groups.DTOs;
using Rekindle.UserGroups.Application.Groups.Exceptions;
using Rekindle.UserGroups.DataAccess;

namespace Rekindle.UserGroups.Application.Groups.Queries;

public record GetGroupMembersQuery(
    Guid GroupId,
    Guid? CurrentUserId = null) : IRequest<List<GroupMemberDto>>;

public class GetGroupMembersQueryHandler : IRequestHandler<GetGroupMembersQuery, List<GroupMemberDto>>
{
    private readonly UserGroupsDbContext _dbContext;

    public GetGroupMembersQueryHandler(UserGroupsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<GroupMemberDto>> Handle(GetGroupMembersQuery request, CancellationToken cancellationToken)
    {
        var group = await _dbContext.Groups
            .FirstOrDefaultAsync(g => g.Id == request.GroupId, cancellationToken);

        if (group == null)
        {
            throw new GroupNotFoundException();
        }

        // Check if current user is a member if provided
        if (request.CurrentUserId.HasValue)
        {
            var isUserMember = await _dbContext.GroupUsers
                .AnyAsync(gu => gu.GroupId == request.GroupId && gu.UserId == request.CurrentUserId, cancellationToken);

            if (!isUserMember)
            {
                throw new NotGroupMemberException();
            }
        }

        var members = await _dbContext.GroupUsers
            .Where(gu => gu.GroupId == request.GroupId)
            .Include(gu => gu.User)
            .OrderBy(gu => gu.Role) // Owners first
            .ThenBy(gu => gu.User.Name)
            .ToListAsync(cancellationToken);

        return members
            .Select(GroupMemberDto.FromGroupUser)
            .ToList();
    }
}
