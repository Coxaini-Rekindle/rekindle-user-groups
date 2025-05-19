using MediatR;
using Microsoft.EntityFrameworkCore;
using Rekindle.UserGroups.Application.Groups.DTOs;
using Rekindle.UserGroups.Application.Groups.Exceptions;
using Rekindle.UserGroups.DataAccess;
using Rekindle.UserGroups.Domain.Entities.GroupUsers.Enumerations;

namespace Rekindle.UserGroups.Application.Groups.Commands;

public record JoinGroupWithLinkCommand(
    string Token,
    Guid CurrentUserId) : IRequest<GroupDto>;

public class JoinGroupWithLinkCommandHandler : IRequestHandler<JoinGroupWithLinkCommand, GroupDto>
{
    private readonly UserGroupsDbContext _dbContext;
    
    public JoinGroupWithLinkCommandHandler(UserGroupsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GroupDto> Handle(JoinGroupWithLinkCommand request, CancellationToken cancellationToken)
    {
        var invitationLink = await _dbContext.InvitationLinks
            .FirstOrDefaultAsync(il => il.Token == request.Token, cancellationToken);
        
        if (invitationLink == null || !invitationLink.IsValid())
        {
            throw new InviteNotFoundException();
        }
        
        var group = await _dbContext.Groups
            .Include(g => g.Members)
            .FirstOrDefaultAsync(g => g.Id == invitationLink.GroupId, cancellationToken);
        
        if (group == null)
        {
            throw new GroupNotFoundException();
        }
        
        // Check if user is already a member
        var existingMember = group.GetMember(request.CurrentUserId);
        if (existingMember != null)
        {
            throw new AlreadyGroupMemberException();
        }
        
        // Get the user
        var user = await _dbContext.Users.FindAsync(new object[] { request.CurrentUserId }, cancellationToken);
        
        if (user == null)
        {
            throw new Authentication.Exceptions.UserNotFoundException();
        }
        
        // Increment invitation link usage count
        if (!invitationLink.IncrementUsageCount())
        {
            throw new InviteNotFoundException();
        }
        
        // Add the user to the group
        group.AddMember(user, GroupUserRole.Member);
        
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return GroupDto.FromGroup(group, request.CurrentUserId);
    }
}
