using MediatR;
using Microsoft.EntityFrameworkCore;
using Rekindle.UserGroups.Application.Groups.DTOs;
using Rekindle.UserGroups.DataAccess;

namespace Rekindle.UserGroups.Application.Groups.Queries;

public record GetUserGroupsQuery(Guid UserId) : IRequest<List<GroupDto>>;

public class GetUserGroupsQueryHandler : IRequestHandler<GetUserGroupsQuery, List<GroupDto>>
{
    private readonly UserGroupsDbContext _dbContext;

    public GetUserGroupsQueryHandler(UserGroupsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<GroupDto>> Handle(GetUserGroupsQuery request, CancellationToken cancellationToken)
    {
        var userGroups = await _dbContext.GroupUsers
            .Where(gu => gu.UserId == request.UserId)
            .Include(gu => gu.Group)
                .ThenInclude(g => g.Members)
            .Select(gu => gu.Group)
            .ToListAsync(cancellationToken);

        return userGroups
            .Select(g => GroupDto.FromGroup(g, request.UserId))
            .ToList();
    }
}
