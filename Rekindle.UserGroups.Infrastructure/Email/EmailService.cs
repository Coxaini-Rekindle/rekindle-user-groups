using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using Rekindle.UserGroups.Application.Common.Interfaces;
using Rekindle.UserGroups.Infrastructure.Email.Settings;

namespace Rekindle.UserGroups.Infrastructure.Email;

public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
    {
        _emailSettings = emailSettings.Value;
        _logger = logger;
    }

    public async Task SendEmailAsync(string to, string subject, string htmlContent, CancellationToken cancellationToken = default)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(_emailSettings.SenderEmail));
        email.To.Add(MailboxAddress.Parse(to));
        email.Subject = subject;

        var builder = new BodyBuilder
        {
            HtmlBody = htmlContent
        };

        email.Body = builder.ToMessageBody();

        try
        {
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.Port, _emailSettings.UseSsl, cancellationToken);
            
            if (!string.IsNullOrEmpty(_emailSettings.Username) && !string.IsNullOrEmpty(_emailSettings.Password))
            {
                await smtp.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password, cancellationToken);
            }
            
            await smtp.SendAsync(email, cancellationToken);
            await smtp.DisconnectAsync(true, cancellationToken);
            
            _logger.LogInformation("Email sent successfully to {Recipient}", to);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {Recipient}", to);
            throw;
        }
    }

    public async Task SendGroupInvitationEmailAsync(string to, string inviterName, string groupName, string invitationLink, CancellationToken cancellationToken = default)
    {
        string subject = $"You've been invited to join {groupName}";
        
        // Create a simple HTML email template
        string htmlContent = $@"
        <!DOCTYPE html>
        <html>
        <head>
            <title>Group Invitation</title>
        </head>
        <body style=""font-family: Arial, sans-serif; line-height: 1.6; color: #333;"">
            <div style=""max-width: 600px; margin: 0 auto; padding: 20px;"">
                <h2 style=""color: #4a6ee0;"">Group Invitation</h2>
                <p>Hello,</p>
                <p><strong>{inviterName}</strong> has invited you to join the group <strong>{groupName}</strong>.</p>
                <p>Click the button below to accept the invitation:</p>
                <p style=""text-align: center;"">
                    <a href=""{invitationLink}"" style=""background-color: #4a6ee0; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px; display: inline-block;"">
                        Accept Invitation
                    </a>
                </p>
                <p>This invitation will expire in 7 days.</p>
                <p>Best regards,<br/>Rekindle Team</p>
            </div>
        </body>
        </html>";
        
        await SendEmailAsync(to, subject, htmlContent, cancellationToken);
    }
}
