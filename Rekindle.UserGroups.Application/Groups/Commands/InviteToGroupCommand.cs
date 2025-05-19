using MediatR;
using Microsoft.EntityFrameworkCore;
using Rekindle.UserGroups.Application.Common.Interfaces;
using Rekindle.UserGroups.Application.Groups.DTOs;
using Rekindle.UserGroups.Application.Groups.Exceptions;
using Rekindle.UserGroups.DataAccess;
using Rekindle.UserGroups.Domain.Entities.GroupInvites;
using Rekindle.UserGroups.Domain.Entities.GroupUsers.Enumerations;

namespace Rekindle.UserGroups.Application.Groups.Commands;

public record InviteToGroupCommand(
    Guid GroupId,
    string Email,
    Guid InvitedByUserId) : IRequest<GroupInviteDto>;

public class InviteToGroupCommandHandler : IRequestHandler<InviteToGroupCommand, GroupInviteDto>
{
    private readonly UserGroupsDbContext _dbContext;
    private readonly IEmailScheduler _emailScheduler;
    
    public InviteToGroupCommandHandler(UserGroupsDbContext dbContext, IEmailScheduler emailScheduler)
    {
        _dbContext = dbContext;
        _emailScheduler = emailScheduler;
    }

    public async Task<GroupInviteDto> Handle(InviteToGroupCommand request, CancellationToken cancellationToken)
    {
        var group = await _dbContext.Groups
            .Include(g => g.Members)
            .FirstOrDefaultAsync(g => g.Id == request.GroupId, cancellationToken);
        
        if (group == null)
        {
            throw new GroupNotFoundException();
        }
        
        var currentUserMember = group.GetMember(request.InvitedByUserId);
        
        if (currentUserMember == null)
        {
            throw new NotGroupMemberException();
        }
        
        // Check if the user being invited already exists in the system
        var invitedUser = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);
        
        // Check if this email is already a member of the group
        if (invitedUser != null)
        {
            var existingMember = group.GetMember(invitedUser.Id);
            if (existingMember != null)
            {
                throw new AlreadyGroupMemberException();
            }
        }
        
        // Check if there's already a pending invite for this email
        var existingInvite = await _dbContext.GroupInvites
            .FirstOrDefaultAsync(gi => 
                gi.GroupId == request.GroupId && 
                gi.Email == request.Email && 
                gi.Status == Domain.Entities.GroupInvites.Enumerations.InvitationStatus.Pending,
                cancellationToken);
        
        if (existingInvite != null)
        {
            // Return the existing invite
            return GroupInviteDto.FromGroupInvite(existingInvite);
        }
        
        // Create the invitation
        var invite = GroupInvite.Create(
            request.GroupId,
            request.InvitedByUserId,
            invitedUser?.Id,
            request.Email
        );
        
        _dbContext.GroupInvites.Add(invite);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        // Load the necessary navigation properties
        await _dbContext.Entry(invite)
            .Reference(i => i.Group)
            .LoadAsync(cancellationToken);
        
        await _dbContext.Entry(invite)
            .Reference(i => i.InvitedBy)
            .LoadAsync(cancellationToken);
        
        // Generate invitation link
        var invitationLink = $"https://rekindle.com/groups/invites/{invite.Id}";
        
        // Schedule invitation email
        await _emailScheduler.ScheduleGroupInvitationEmailAsync(
            request.Email,
            invite.InvitedBy.Name,
            group.Name,
            invitationLink
        );
        
        return GroupInviteDto.FromGroupInvite(invite);
    }
}
