namespace Rekindle.UserGroups.Application.Common.Interfaces;

public interface IEmailScheduler
{
    Task ScheduleEmailAsync(string to, string subject, string htmlContent, TimeSpan? delay = null);

    Task ScheduleGroupInvitationEmailAsync(string to, string inviterName, string groupName, string invitationLink,
        TimeSpan? delay = null);
}