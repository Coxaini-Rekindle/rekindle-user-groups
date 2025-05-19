namespace Rekindle.UserGroups.Application.Common.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string htmlContent, CancellationToken cancellationToken = default);

    Task SendGroupInvitationEmailAsync(string to, string inviterName, string groupName, string invitationLink,
        CancellationToken cancellationToken = default);
}