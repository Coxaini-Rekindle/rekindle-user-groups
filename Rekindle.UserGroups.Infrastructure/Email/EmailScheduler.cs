using Microsoft.Extensions.Logging;
using Quartz;
using Rekindle.UserGroups.Application.Common.Interfaces;
using Rekindle.UserGroups.Infrastructure.Email.Jobs;

namespace Rekindle.UserGroups.Infrastructure.Email;

public class EmailScheduler : IEmailScheduler
{
    private readonly ISchedulerFactory _schedulerFactory;
    private readonly ILogger<EmailScheduler> _logger;

    public EmailScheduler(ISchedulerFactory schedulerFactory, ILogger<EmailScheduler> logger)
    {
        _schedulerFactory = schedulerFactory;
        _logger = logger;
    }

    public async Task ScheduleEmailAsync(string to, string subject, string htmlContent, TimeSpan? delay = null)
    {
        try
        {
            var scheduler = await _schedulerFactory.GetScheduler();
            
            var jobData = new JobDataMap
            {
                { SendEmailJob.JobKeys.RecipientEmail, to },
                { SendEmailJob.JobKeys.Subject, subject },
                { SendEmailJob.JobKeys.HtmlContent, htmlContent },
                { SendEmailJob.JobKeys.IsGroupInvitation, false }
            };
              var jobKey = new JobKey($"email-job-{Guid.NewGuid()}");
            var job = JobBuilder.Create<SendEmailJob>()
                .WithIdentity(jobKey)
                .UsingJobData(jobData)
                .StoreDurably(true)
                .Build();

            var triggerBuilder = TriggerBuilder.Create()
                .WithIdentity($"email-trigger-{Guid.NewGuid()}")
                .ForJob(jobKey);

            if (delay.HasValue && delay.Value > TimeSpan.Zero)
            {
                triggerBuilder.StartAt(DateTimeOffset.UtcNow.Add(delay.Value));
            }
            else
            {
                triggerBuilder.StartNow();
            }

            var trigger = triggerBuilder.Build();
            
            await scheduler.ScheduleJob(job, trigger);
            
            _logger.LogInformation("Email to {Recipient} scheduled successfully", to);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to schedule email to {Recipient}", to);
            throw;
        }
    }

    public async Task ScheduleGroupInvitationEmailAsync(string to, string inviterName, string groupName, string invitationLink, TimeSpan? delay = null)
    {
        try
        {
            var scheduler = await _schedulerFactory.GetScheduler();
            
            var jobData = new JobDataMap
            {
                { SendEmailJob.JobKeys.RecipientEmail, to },
                { SendEmailJob.JobKeys.InviterName, inviterName },
                { SendEmailJob.JobKeys.GroupName, groupName },
                { SendEmailJob.JobKeys.InvitationLink, invitationLink },
                { SendEmailJob.JobKeys.IsGroupInvitation, true }
            };
              var jobKey = new JobKey($"group-invitation-job-{Guid.NewGuid()}");
            var job = JobBuilder.Create<SendEmailJob>()
                .WithIdentity(jobKey)
                .UsingJobData(jobData)
                .StoreDurably(true)
                .Build();

            var triggerBuilder = TriggerBuilder.Create()
                .WithIdentity($"group-invitation-trigger-{Guid.NewGuid()}")
                .ForJob(jobKey);

            if (delay.HasValue && delay.Value > TimeSpan.Zero)
            {
                triggerBuilder.StartAt(DateTimeOffset.UtcNow.Add(delay.Value));
            }
            else
            {
                triggerBuilder.StartNow();
            }

            var trigger = triggerBuilder.Build();
            
            await scheduler.ScheduleJob(job, trigger);
            
            _logger.LogInformation("Group invitation email to {Recipient} for group {GroupName} scheduled successfully", to, groupName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to schedule group invitation email to {Recipient}", to);
            throw;
        }
    }
}
