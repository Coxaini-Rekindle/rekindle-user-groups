using MediatR;
using Microsoft.EntityFrameworkCore;
using Rekindle.UserGroups.Application.Groups.DTOs;
using Rekindle.UserGroups.Application.Groups.Exceptions;
using Rekindle.UserGroups.DataAccess;
using Rekindle.UserGroups.Domain.Entities.GroupUsers.Enumerations;

namespace Rekindle.UserGroups.Application.Groups.Commands;

public record UpdateGroupCommand(
    Guid GroupId,
    string Name,
    string Description,
    Guid CurrentUserId) : IRequest<GroupDto>;

public class UpdateGroupCommandHandler : IRequestHandler<UpdateGroupCommand, GroupDto>
{
    private readonly UserGroupsDbContext _dbContext;
    
    public UpdateGroupCommandHandler(UserGroupsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GroupDto> Handle(UpdateGroupCommand request, CancellationToken cancellationToken)
    {
        var group = await _dbContext.Groups
            .Include(g => g.Members)
            .ThenInclude(m => m.User)
            .FirstOrDefaultAsync(g => g.Id == request.GroupId, cancellationToken);
        
        if (group == null)
        {
            throw new GroupNotFoundException();
        }
        
        var currentUserMember = group.GetMember(request.CurrentUserId);
        
        if (currentUserMember == null)
        {
            throw new NotGroupMemberException();
        }
        
        if (currentUserMember.Role != GroupUserRole.Owner)
        {
            throw new NotGroupOwnerException();
        }        
        group.Update(request.Name, request.Description, request.CurrentUserId);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return GroupDto.FromGroup(group, request.CurrentUserId);
    }
}
