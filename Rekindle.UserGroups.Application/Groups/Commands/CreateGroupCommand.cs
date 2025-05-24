using MediatR;
using Microsoft.EntityFrameworkCore;
using Rekindle.UserGroups.Application.Common.Interfaces;
using Rekindle.UserGroups.Application.Groups.DTOs;
using Rekindle.UserGroups.DataAccess;
using Rekindle.UserGroups.Domain.Entities.Groups;

namespace Rekindle.UserGroups.Application.Groups.Commands;

public record CreateGroupCommand(
    string Name,
    string Description,
    Guid CreatedByUserId,
    IEnumerable<string> InvitationEmails
) : IRequest<GroupDto>;

public class CreateGroupCommandHandler : IRequestHandler<CreateGroupCommand, GroupDto>
{
    private readonly UserGroupsDbContext _dbContext;
    private readonly IGroupInvitationService _groupInvitationService;

    public CreateGroupCommandHandler(
        UserGroupsDbContext dbContext,
        IGroupInvitationService groupInvitationService)
    {
        _dbContext = dbContext;
        _groupInvitationService = groupInvitationService;
    }

    public async Task<GroupDto> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FindAsync([request.CreatedByUserId], cancellationToken);
        
        if (user == null)
        {
            throw new Authentication.Exceptions.UserNotFoundException();
        }
        
        var group = Group.Create(
            request.Name,
            request.Description,
            DateTime.UtcNow,
            user
        );
        
        _dbContext.Groups.Add(group);
        await _dbContext.SaveChangesAsync(cancellationToken);

        // Load members for the invitation service
        await _dbContext.Entry(group)
            .Collection(g => g.Members)
            .LoadAsync(cancellationToken);
        
        // Send invitations to the provided emails
        if (request.InvitationEmails?.Any() == true)
        {
            await _groupInvitationService.CreateInvitationsForGroupAsync(
                group,
                request.InvitationEmails,
                request.CreatedByUserId,
                cancellationToken);
        }
        
        return GroupDto.FromGroup(group, request.CreatedByUserId);
    }
}
