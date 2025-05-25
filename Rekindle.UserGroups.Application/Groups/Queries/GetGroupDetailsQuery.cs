using MediatR;
using Microsoft.EntityFrameworkCore;
using Rekindle.UserGroups.Application.Groups.DTOs;
using Rekindle.UserGroups.Application.Groups.Exceptions;
using Rekindle.UserGroups.DataAccess;

namespace Rekindle.UserGroups.Application.Groups.Queries;

public record GetGroupDetailsQuery(
    Guid GroupId, 
    Guid? CurrentUserId = null) : IRequest<GroupDto>;

public class GetGroupDetailsQueryHandler : IRequestHandler<GetGroupDetailsQuery, GroupDto>
{
    private readonly UserGroupsDbContext _dbContext;

    public GetGroupDetailsQueryHandler(UserGroupsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GroupDto> Handle(GetGroupDetailsQuery request, CancellationToken cancellationToken)
    {
        var group = await _dbContext.Groups
            .Include(g => g.Members)
            .ThenInclude(m => m.User)
            .FirstOrDefaultAsync(g => g.Id == request.GroupId, cancellationToken);

        if (group == null)
        {
            throw new GroupNotFoundException();
        }

        return GroupDto.FromGroup(group, request.CurrentUserId);
    }
}
