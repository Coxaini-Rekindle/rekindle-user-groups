using Microsoft.EntityFrameworkCore;
using Rekindle.UserGroups.Domain.Entities.GroupInvites;
using Rekindle.UserGroups.Domain.Entities.Groups;
using Rekindle.UserGroups.Domain.Entities.GroupUsers;
using Rekindle.UserGroups.Domain.Entities.Users;

namespace Rekindle.UserGroups.DataAccess;

public class UserGroupsDbContext : DbContext
{
    public UserGroupsDbContext(DbContextOptions<UserGroupsDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Group> Groups { get; set; } = null!;
    public DbSet<GroupUser> GroupUsers { get; set; } = null!;
    public DbSet<GroupInvite> GroupInvites { get; set; } = null!;
    public DbSet<InvitationLink> InvitationLinks { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserGroupsDbContext).Assembly);
    }
}