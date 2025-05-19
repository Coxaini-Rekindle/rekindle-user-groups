using MediatR;
using Microsoft.EntityFrameworkCore;
using Rekindle.UserGroups.Application.Groups.Exceptions;
using Rekindle.UserGroups.DataAccess;
using Rekindle.UserGroups.Domain.Entities.GroupInvites;
using Rekindle.UserGroups.Domain.Entities.GroupUsers.Enumerations;

namespace Rekindle.UserGroups.Application.Groups.Commands;

public record CreateInviteLinkCommand(
    Guid GroupId,
    int MaxUses,
    int ExpirationDays,
    Guid CurrentUserId) : IRequest<string>;

public class CreateInviteLinkCommandHandler : IRequestHandler<CreateInviteLinkCommand, string>
{
    private readonly UserGroupsDbContext _dbContext;
    
    public CreateInviteLinkCommandHandler(UserGroupsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<string> Handle(CreateInviteLinkCommand request, CancellationToken cancellationToken)
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
        
        // Create invitation link
        var invitationLink = InvitationLink.Create(
            request.GroupId,
            request.CurrentUserId,
            request.MaxUses,
            request.ExpirationDays
        );
        
        _dbContext.InvitationLinks.Add(invitationLink);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        // Return the invitation link URL
        return $"https://rekindle.com/groups/join/{invitationLink.Token}";
    }
}
