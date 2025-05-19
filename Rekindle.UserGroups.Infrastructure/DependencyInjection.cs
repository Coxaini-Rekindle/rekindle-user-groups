using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Quartz;
using Rekindle.UserGroups.Application.Authentication.Interfaces;
using Rekindle.UserGroups.Application.Common.Interfaces;
using Rekindle.UserGroups.Infrastructure.Email;
using Rekindle.UserGroups.Infrastructure.Email.Extensions;
using Rekindle.UserGroups.Infrastructure.Email.Jobs;
using Rekindle.UserGroups.Infrastructure.Email.Settings;
using Rekindle.UserGroups.Infrastructure.Security;
using Rekindle.UserGroups.Infrastructure.Security.Jwt;

namespace Rekindle.UserGroups.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddJwtAuth(configuration);
        services.AddEmailServices(configuration);
        
        return services;
    }

    public static IServiceCollection AddJwtAuth(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = new JwtSettings();
        configuration.Bind("JwtSettings", jwtSettings);

        services.AddSingleton(Options.Create(jwtSettings));

        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddSingleton<IRefreshTokenGenerator, RefreshTokenGenerator>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings.Secret))
                };
            });

        return services;
    }
    
    public static IServiceCollection AddEmailServices(this IServiceCollection services, IConfiguration configuration)
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