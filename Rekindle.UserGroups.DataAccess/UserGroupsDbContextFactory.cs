using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using MediatR;

namespace Rekindle.UserGroups.DataAccess;

public class UserGroupsDbContextFactory : IDesignTimeDbContextFactory<UserGroupsDbContext>
{
    public UserGroupsDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<UserGroupsDbContext>();
        // Use a default connection string for design-time operations
        var connectionString = "Host=localhost;Port=5432;Database=user_groups;Username=postgres;Password=password";
        optionsBuilder.UseNpgsql(connectionString,
            options =>
            {
                options.MigrationsHistoryTable($"__{nameof(UserGroupsDbContext)}MigrationsHistory");
                options.MigrationsAssembly(typeof(DependencyInjection).Assembly.GetName().Name);
            });

        // Create a dummy publisher for design-time
        var dummyPublisher = new DummyPublisher();

        return new UserGroupsDbContext(optionsBuilder.Options, dummyPublisher);
    }

    private class DummyPublisher : IPublisher
    {
        public Task Publish(object notification, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
            where TNotification : INotification
        {
            return Task.CompletedTask;
        }
    }
}