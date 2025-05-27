using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rekindle.UserGroups.Infrastructure.Messaging;
using Quartz;
using Rekindle.Authentication;
using Rekindle.UserGroups.Application.Common.Interfaces;
using Rekindle.UserGroups.Application.Storage.Interfaces;
using Rekindle.UserGroups.Infrastructure.Email;
using Rekindle.UserGroups.Infrastructure.Email.Extensions;
using Rekindle.UserGroups.Infrastructure.Email.Jobs;
using Rekindle.UserGroups.Infrastructure.Email.Settings;
using Rekindle.UserGroups.Infrastructure.Storage;

namespace Rekindle.UserGroups.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddJwtAuth(configuration);
        services.AddEmailServices(configuration);
        services.AddRebusMessageBus(configuration);
        services.AddFileStorage(configuration);
        
        return services;
    }

    private static IServiceCollection AddFileStorage(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IFileStorage, FileStorage>();
        services.AddSingleton(_ => new BlobServiceClient(configuration.GetConnectionString("BlobStorage")));

        return services;
    }

    private static IServiceCollection AddEmailServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Register email settings
        var emailSettings = new EmailSettings();
        configuration.GetSection("EmailSettings").Bind(emailSettings);
        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
          // Register email service
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IEmailScheduler, EmailScheduler>();
          // Configure Quartz
        services.AddQuartz(q =>
        {
            // Register the job class without creating a default trigger
            q.AddJobAndTrigger<SendEmailJob>(configuration);
            
            // Configure persistent storage if needed
            // q.UsePersistentStore(s => { ... });
        });
        
        // Add the Quartz.NET hosted service
        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
        
        return services;
    }
}