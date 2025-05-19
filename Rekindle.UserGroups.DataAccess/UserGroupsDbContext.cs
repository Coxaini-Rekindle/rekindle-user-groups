using MediatR;
using Microsoft.EntityFrameworkCore;
using Rekindle.UserGroups.Domain.Common;
using Rekindle.UserGroups.Domain.Entities.GroupInvites;
using Rekindle.UserGroups.Domain.Entities.Groups;
using Rekindle.UserGroups.Domain.Entities.GroupUsers;
using Rekindle.UserGroups.Domain.Entities.Users;

namespace Rekindle.UserGroups.DataAccess;

public class UserGroupsDbContext : DbContext
{
    private readonly IPublisher _publisher;

    public UserGroupsDbContext(DbContextOptions<UserGroupsDbContext> options, IPublisher publisher) : base(options)
    {
        _publisher = publisher;
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Group> Groups { get; set; } = null!;
    public DbSet<GroupUser> GroupUsers { get; set; } = null!;
    public DbSet<GroupInvite> GroupInvites { get; set; } = null!;
    public DbSet<InvitationLink> InvitationLinks { get; set; } = null!;    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserGroupsDbContext).Assembly);
        
        // Ignore DomainEvent class - it's not an entity that should be stored in the database
        modelBuilder.Ignore<DomainEvent>();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Save changes to the database first
        var result = await base.SaveChangesAsync(cancellationToken);

        // Then dispatch domain events after successful save
        await DispatchDomainEventsAsync(cancellationToken);

        return result;
    }

    private async Task DispatchDomainEventsAsync(CancellationToken cancellationToken)
    {
        // Find all entities that might have domain events
        var entities = ChangeTracker
            .Entries<Entity>()
            .Where(e => e.Entity.DomainEvents.Count != 0)
            .Select(e => e.Entity)
            .ToList();

        // Get all domain events from all entities
        var domainEvents = entities
            .SelectMany(e => e.DomainEvents)
            .ToList();

        // Early exit if no events to dispatch
        if (domainEvents.Count == 0) return;

        // Clear domain events from entities
        entities.ForEach(e => e.ClearDomainEvents());

        // Dispatch all events
        foreach (var domainEvent in domainEvents) await _publisher.Publish(domainEvent, cancellationToken);
    }
}