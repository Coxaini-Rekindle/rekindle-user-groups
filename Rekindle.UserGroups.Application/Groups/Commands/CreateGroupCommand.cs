using MediatR;
using Microsoft.EntityFrameworkCore;
using Rekindle.UserGroups.Application.Groups.DTOs;
using Rekindle.UserGroups.Domain.Entities.Groups;
using Rekindle.UserGroups.Domain.Entities.GroupUsers.Enumerations;
using Rekindle.UserGroups.DataAccess;

namespace Rekindle.UserGroups.Application.Groups.Commands;

public record CreateGroupCommand(
    string Name, 
    string Description, 
    Guid CreatedByUserId) : IRequest<GroupDto>;

public class CreateGroupCommandHandler : IRequestHandler<CreateGroupCommand, GroupDto>
{
    private readonly UserGroupsDbContext _dbContext;
    
    public CreateGroupCommandHandler(UserGroupsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GroupDto> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FindAsync(new object[] { request.CreatedByUserId }, cancellationToken);
        
        if (user == null)
        {
            throw new Authentication.Exceptions.UserNotFoundException();
        }
        
        var group = Group.Create(
            request.Name,
            request.Description,
            DateTime.UtcNow
        );
        
        // Add creator as owner
        group.AddMember(user, GroupUserRole.Owner);
        
        _dbContext.Groups.Add(group);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return GroupDto.FromGroup(group, request.CreatedByUserId);
    }
}
