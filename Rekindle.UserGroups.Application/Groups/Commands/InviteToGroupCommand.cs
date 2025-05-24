using MediatR;
using Rekindle.UserGroups.Application.Common.Interfaces;
using Rekindle.UserGroups.Application.Groups.DTOs;

namespace Rekindle.UserGroups.Application.Groups.Commands;

public record InviteToGroupCommand(
    Guid GroupId,
    string Email,
    Guid InvitedByUserId) : IRequest<GroupInviteDto>;

public class InviteToGroupCommandHandler : IRequestHandler<InviteToGroupCommand, GroupInviteDto>
{
    private readonly IGroupInvitationService _groupInvitationService;
    
    public InviteToGroupCommandHandler(IGroupInvitationService groupInvitationService)
    {
        _groupInvitationService = groupInvitationService;
    }

    public async Task<GroupInviteDto> Handle(InviteToGroupCommand request, CancellationToken cancellationToken)
    {
        return await _groupInvitationService.InviteToGroupAsync(
            request.GroupId,
            request.Email,
            request.InvitedByUserId,
            cancellationToken);
    }
}
