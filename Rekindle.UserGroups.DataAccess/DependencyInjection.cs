using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Rekindle.UserGroups.DataAccess;

public static class DependencyInjection
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        Guard.Against.NullOrEmpty(connectionString);

        services.AddDbContext<UserGroupsDbContext>(options =>
        {
            options.UseNpgsql(connectionString, sqlOptions =>
            {
                sqlOptions.MigrationsHistoryTable($"__{nameof(UserGroupsDbContext)}MigrationsHistory");
                sqlOptions.MigrationsAssembly(typeof(DependencyInjection).Assembly.GetName().Name);
            });
        });

        using var scope = services.BuildServiceProvider().CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<UserGroupsDbContext>();

        dbContext.Database.Migrate();

        return services;
    }
}