using Microsoft.Extensions.DependencyInjection;
using Rekindle.UserGroups.Application.Common.Interfaces;
using Rekindle.UserGroups.Application.Groups.Services;

namespace Rekindle.UserGroups.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => { cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly); });
        
        // Register application services
        services.AddScoped<IGroupInvitationService, GroupInvitationService>();
        
        return services;
    }
}