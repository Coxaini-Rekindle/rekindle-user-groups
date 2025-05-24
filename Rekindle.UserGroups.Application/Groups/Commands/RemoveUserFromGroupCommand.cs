using MediatR;
using Microsoft.EntityFrameworkCore;
using Rekindle.UserGroups.Application.Groups.Exceptions;
using Rekindle.UserGroups.DataAccess;
using Rekindle.UserGroups.Domain.Entities.GroupUsers.Enumerations;

namespace Rekindle.UserGroups.Application.Groups.Commands;

public record RemoveUserFromGroupCommand(
    Guid GroupId,
    Guid UserIdToRemove,
    Guid CurrentUserId) : IRequest<bool>;

public class RemoveUserFromGroupCommandHandler : IRequestHandler<RemoveUserFromGroupCommand, bool>
{
    private readonly UserGroupsDbContext _dbContext;

    public RemoveUserFromGroupCommandHandler(UserGroupsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> Handle(RemoveUserFromGroupCommand request, CancellationToken cancellationToken)
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

        var targetMember = group.GetMember(request.UserIdToRemove);
        if (targetMember == null)
        {
            throw new NotGroupMemberException();
        }

        // Prevent removing the group owner
        if (targetMember.Role == GroupUserRole.Owner)
        {
            throw new CannotRemoveGroupOwnerException();
        }

        // Remove the user from the group
        var wasRemoved = group.RemoveMember(request.UserIdToRemove, wasRemoved: true, removedByUserId: request.CurrentUserId);
        
        if (wasRemoved)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        return wasRemoved;
    }
}
