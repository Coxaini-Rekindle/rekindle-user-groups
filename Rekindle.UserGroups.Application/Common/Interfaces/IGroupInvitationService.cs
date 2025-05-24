using Rekindle.UserGroups.Application.Groups.DTOs;
using Rekindle.UserGroups.Domain.Entities.Groups;

namespace Rekindle.UserGroups.Application.Common.Interfaces;

public interface IGroupInvitationService
{
    /// <summary>
    /// Creates an invitation for a single email to join a group
    /// </summary>
    /// <param name="groupId">The ID of the group</param>
    /// <param name="email">Email address to invite</param>
    /// <param name="invitedByUserId">ID of the user sending the invitation</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created group invitation DTO</returns>
    Task<GroupInviteDto> InviteToGroupAsync(
        Guid groupId, 
        string email, 
        Guid invitedByUserId, 
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates invitations for multiple emails to join a group (used during group creation)
    /// </summary>
    /// <param name="group">The group entity (must have members populated)</param>
    /// <param name="emails">Collection of email addresses to invite</param>
    /// <param name="invitedByUserId">ID of the user sending the invitations</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Collection of created group invitation DTOs</returns>
    Task<IEnumerable<GroupInviteDto>> CreateInvitationsForGroupAsync(
        Group group,
        IEnumerable<string> emails, 
        Guid invitedByUserId, 
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends an invitation email for an existing invitation
    /// </summary>
    /// <param name="invitation">The invitation DTO</param>
    /// <param name="groupName">Name of the group</param>
    /// <param name="inviterName">Name of the person sending the invitation</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task SendInvitationEmailAsync(
        GroupInviteDto invitation,
        string groupName,
        string inviterName,
        CancellationToken cancellationToken = default);
}
