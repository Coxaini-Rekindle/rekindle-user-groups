using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rekindle.UserGroups.Domain.Entities.GroupInvites;

namespace Rekindle.UserGroups.DataAccess.Configurations;

public class GroupInviteEntityTypeConfiguration : IEntityTypeConfiguration<GroupInvite>
{
    public void Configure(EntityTypeBuilder<GroupInvite> builder)
    {
        builder.HasKey(gi => gi.Id);
        
        builder.Property(gi => gi.Email)
            .HasMaxLength(255);
            
        builder.Property(gi => gi.Status)
            .IsRequired();
            
        builder.Property(gi => gi.CreatedAt)
            .IsRequired();
            
        builder.Property(gi => gi.ExpiresAt)
            .IsRequired();
            
        // Define relationship with Group
        builder.HasOne(gi => gi.Group)
            .WithMany()
            .HasForeignKey(gi => gi.GroupId)
            .OnDelete(DeleteBehavior.Cascade);
            
        // Define relationship with InvitedBy user
        builder.HasOne(gi => gi.InvitedBy)
            .WithMany()
            .HasForeignKey(gi => gi.InvitedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
            
        // Define relationship with InvitedUser (optional)
        builder.HasOne(gi => gi.InvitedUser)
            .WithMany()
            .HasForeignKey(gi => gi.InvitedUserId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
