using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Rekindle.UserGroups.Application.Common.Configs;
using Rekindle.UserGroups.Application.Common.Interfaces;
using Rekindle.UserGroups.Application.Groups.DTOs;
using Rekindle.UserGroups.Application.Groups.Exceptions;
using Rekindle.UserGroups.DataAccess;
using Rekindle.UserGroups.Domain.Entities.GroupInvites;
using Rekindle.UserGroups.Domain.Entities.Groups;

namespace Rekindle.UserGroups.Application.Groups.Services;

public class GroupInvitationService : IGroupInvitationService
{
    private readonly UserGroupsDbContext _dbContext;
    private readonly IEmailScheduler _emailScheduler;
    private readonly FrontendConfig _frontendConfig;

    public GroupInvitationService(UserGroupsDbContext dbContext, IEmailScheduler emailScheduler,
        IOptions<FrontendConfig> frontendConfig)
    {
        _dbContext = dbContext;
        _emailScheduler = emailScheduler;
        _frontendConfig = frontendConfig.Value;
    }

    public async Task<GroupInviteDto> InviteToGroupAsync(
        Guid groupId, 
        string email, 
        Guid invitedByUserId, 
        CancellationToken cancellationToken = default)
    {
        var group = await _dbContext.Groups
            .Include(g => g.Members)
            .FirstOrDefaultAsync(g => g.Id == groupId, cancellationToken);
        
        if (group == null)
        {
            throw new GroupNotFoundException();
        }
        
        var currentUserMember = group.GetMember(invitedByUserId);
        
        if (currentUserMember == null)
        {
            throw new NotGroupMemberException();
        }
        
        // Check if the user being invited already exists in the system
        var invitedUser = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        
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
                gi.GroupId == groupId && 
                gi.Email == email && 
                gi.Status == Domain.Entities.GroupInvites.Enumerations.InvitationStatus.Pending,
                cancellationToken);
        
        if (existingInvite != null)
        {
            // Return the existing invite
            return GroupInviteDto.FromGroupInvite(existingInvite);
        }
        
        // Create the invitation
        var invite = GroupInvite.Create(
            groupId,
            invitedByUserId,
            invitedUser?.Id,
            email
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
        
        // Generate invitation link and send email
        var invitationLink = $"{_frontendConfig.PersonalInvitationUrl}{invite.Id}";
        
        await _emailScheduler.ScheduleGroupInvitationEmailAsync(
            email,
            invite.InvitedBy.Name,
            group.Name,
            invitationLink
        );
        
        return GroupInviteDto.FromGroupInvite(invite);
    }

    public async Task<IEnumerable<GroupInviteDto>> CreateInvitationsForGroupAsync(
        Group group,
        IEnumerable<string> emails, 
        Guid invitedByUserId, 
        CancellationToken cancellationToken = default)
    {
        if (!emails.Any())
        {
            return Enumerable.Empty<GroupInviteDto>();
        }

        var invitationDtos = new List<GroupInviteDto>();
        var inviterUser = await _dbContext.Users.FindAsync([invitedByUserId], cancellationToken);
        
        if (inviterUser == null)
        {
            throw new Authentication.Exceptions.UserNotFoundException();
        }

        // Process each email
        foreach (var email in emails.Where(e => !string.IsNullOrWhiteSpace(e)))
        {
            try
            {
                // Check if the user being invited already exists in the system
                var invitedUser = await _dbContext.Users
                    .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
                
                // Check if this email is already a member of the group (skip if so)
                if (invitedUser != null)
                {
                    var existingMember = group.GetMember(invitedUser.Id);
                    if (existingMember != null)
                    {
                        continue; // Skip this email, already a member
                    }
                }
                
                // Check if there's already a pending invite for this email (skip if so)
                var existingInvite = await _dbContext.GroupInvites
                    .FirstOrDefaultAsync(gi => 
                        gi.GroupId == group.Id && 
                        gi.Email == email && 
                        gi.Status == Domain.Entities.GroupInvites.Enumerations.InvitationStatus.Pending,
                        cancellationToken);
                
                if (existingInvite != null)
                {
                    continue; // Skip this email, already has pending invite
                }
                
                // Create the invitation
                var invite = GroupInvite.Create(
                    group.Id,
                    invitedByUserId,
                    invitedUser?.Id,
                    email
                );
                
                _dbContext.GroupInvites.Add(invite);
                
                // Note: We'll save all invitations together and send emails after
                invitationDtos.Add(new GroupInviteDto
                {
                    Id = invite.Id,
                    GroupId = invite.GroupId,
                    GroupName = group.Name,
                    InvitedByUsername = inviterUser.Username,
                    InvitedUsername = invitedUser?.Username,
                    Email = invite.Email,
                    Status = invite.Status,
                    CreatedAt = invite.CreatedAt,
                    ExpiresAt = invite.ExpiresAt
                });
            }
            catch
            {
                // Skip invalid emails or other errors during individual invitation creation
                continue;
            }
        }

        // Save all invitations
        if (invitationDtos.Any())
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
            
            // Send emails for all created invitations
            foreach (var inviteDto in invitationDtos)
            {
                await SendInvitationEmailAsync(inviteDto, group.Name, inviterUser.Name, cancellationToken);
            }
        }

        return invitationDtos;
    }

    public async Task SendInvitationEmailAsync(
        GroupInviteDto invitation,
        string groupName,
        string inviterName,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(invitation.Email))
        {
            return; // Cannot send email without email address
        }

        var invitationLink = $"{_frontendConfig.PersonalInvitationUrl}{invitation.Id}";
        
        await _emailScheduler.ScheduleGroupInvitationEmailAsync(
            invitation.Email,
            inviterName,
            groupName,
            invitationLink
        );
    }
}
