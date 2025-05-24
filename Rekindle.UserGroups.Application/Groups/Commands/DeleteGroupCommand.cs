using MediatR;
using Microsoft.EntityFrameworkCore;
using Rekindle.UserGroups.Application.Groups.Exceptions;
using Rekindle.UserGroups.DataAccess;
using Rekindle.UserGroups.Domain.Entities.GroupUsers.Enumerations;

namespace Rekindle.UserGroups.Application.Groups.Commands;

public record DeleteGroupCommand(
    Guid GroupId,
    Guid CurrentUserId) : IRequest<bool>;

public class DeleteGroupCommandHandler : IRequestHandler<DeleteGroupCommand, bool>
{
    private readonly UserGroupsDbContext _dbContext;

    public DeleteGroupCommandHandler(UserGroupsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> Handle(DeleteGroupCommand request, CancellationToken cancellationToken)
    {
        var group = await _dbContext.Groups
            .Include(g => g.Members)
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

        // Call the domain method to trigger the domain event
        group.Delete(request.CurrentUserId);
        
        // Remove the group from the database
        _dbContext.Groups.Remove(group);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}
