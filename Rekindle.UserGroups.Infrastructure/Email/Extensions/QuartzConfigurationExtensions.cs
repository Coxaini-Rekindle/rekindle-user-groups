using Microsoft.Extensions.Configuration;
using Quartz;

namespace Rekindle.UserGroups.Infrastructure.Email.Extensions;

public static class QuartzConfigurationExtensions
{    public static void AddJob<T>(this IServiceCollectionQuartzConfigurator quartz)
        where T : IJob
    {
        // Register the job without creating any triggers
        var jobKey = new JobKey(typeof(T).Name);
        quartz.AddJob<T>(opts => opts
            .WithIdentity(jobKey)
            .StoreDurably(true)); // Mark as durable since it has no triggers
    }
    
    public static void AddJobAndTrigger<T>(this IServiceCollectionQuartzConfigurator quartz, IConfiguration config)
        where T : IJob
    {
        // Just register the job without a trigger
        // We'll create triggers dynamically when jobs are scheduled
        var jobKey = new JobKey(typeof(T).Name);
        quartz.AddJob<T>(opts => opts
            .WithIdentity(jobKey)
            .StoreDurably(true)); // Mark as durable since it has no triggers
    }
}
