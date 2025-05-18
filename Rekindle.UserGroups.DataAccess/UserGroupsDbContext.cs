using Microsoft.EntityFrameworkCore;
using Rekindle.UserGroups.Domain.Entities.Users;

namespace Rekindle.UserGroups.DataAccess;

public class UserGroupsDbContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserGroupsDbContext).Assembly);
    }
}