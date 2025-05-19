using MediatR;
using Microsoft.EntityFrameworkCore;
using Rekindle.UserGroups.Application.Groups.DTOs;
using Rekindle.UserGroups.Application.Groups.Exceptions;
using Rekindle.UserGroups.DataAccess;
using Rekindle.UserGroups.Domain.Entities.GroupInvites.Enumerations;
using Rekindle.UserGroups.Domain.Entities.GroupUsers.Enumerations;

namespace Rekindle.UserGroups.Application.Groups.Commands;

public record AcceptInviteCommand(
    Guid InviteId,
    Guid CurrentUserId) : IRequest<GroupDto>;

public class AcceptInviteCommandHandler : IRequestHandler<AcceptInviteCommand, GroupDto>
{
    private readonly UserGroupsDbContext _dbContext;
    
    public AcceptInviteCommandHandler(UserGroupsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GroupDto> Handle(AcceptInviteCommand request, CancellationToken cancellationToken)
    {
        var invite = await _dbContext.GroupInvites
            .Include(i => i.Group)
            .FirstOrDefaultAsync(i => i.Id == request.InviteId, cancellationToken);
        
        if (invite == null || invite.IsExpired())
        {
            throw new InviteNotFoundException();
        }
        
        // Verify this invite is for the current user or email
        var currentUser = await _dbContext.Users.FindAsync(new object[] { request.CurrentUserId }, cancellationToken);
        if (currentUser == null)
        {
            throw new Authentication.Exceptions.UserNotFoundException();
        }
        
        if (invite.InvitedUserId.HasValue && invite.InvitedUserId != request.CurrentUserId)
        {
            throw new InviteNotFoundException();
        }
        
        if (!invite.InvitedUserId.HasValue && !string.IsNullOrEmpty(invite.Email) && 
            invite.Email != currentUser.Email)
        {
            throw new InviteNotFoundException();
        }
        
        // Check if user is already a member of the group
        var group = await _dbContext.Groups
            .Include(g => g.Members)
            .FirstOrDefaultAsync(g => g.Id == invite.GroupId, cancellationToken);
        
        if (group == null)
        {
            throw new GroupNotFoundException();
        }
        
        var existingMember = group.GetMember(request.CurrentUserId);
        if (existingMember != null)
        {
            throw new AlreadyGroupMemberException();
        }
        
        // Accept the invitation
        if (!invite.Accept())
        {
            throw new InviteNotFoundException();
        }
        
        // If the invite was using an email, assign it to this user
        if (!invite.InvitedUserId.HasValue && !string.IsNullOrEmpty(invite.Email))
        {
            invite.AssignToUser(request.CurrentUserId);
        }
        
        // Add the user to the group
        group.AddMember(currentUser, GroupUserRole.Member);
        
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return GroupDto.FromGroup(group, request.CurrentUserId);
    }
}
