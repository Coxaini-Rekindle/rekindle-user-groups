using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rekindle.UserGroups.Application.Common.Configs;
using Rekindle.UserGroups.Application.Common.Interfaces;
using Rekindle.UserGroups.Application.Groups.Services;

namespace Rekindle.UserGroups.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(cfg => { cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly); });
        
        // Register application services
        services.AddScoped<IGroupInvitationService, GroupInvitationService>();

        services.Configure<FrontendConfig>(
            configuration.GetSection(FrontendConfig.Frontend));
        
        return services;
    }
}