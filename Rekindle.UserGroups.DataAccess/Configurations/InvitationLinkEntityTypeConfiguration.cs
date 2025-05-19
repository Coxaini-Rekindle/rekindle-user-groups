using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rekindle.UserGroups.Domain.Entities.GroupInvites;

namespace Rekindle.UserGroups.DataAccess.Configurations;

public class InvitationLinkEntityTypeConfiguration : IEntityTypeConfiguration<InvitationLink>
{
    public void Configure(EntityTypeBuilder<InvitationLink> builder)
    {
        builder.HasKey(il => il.Id);
        
        builder.Property(il => il.Token)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.HasIndex(il => il.Token)
            .IsUnique();
            
        builder.Property(il => il.MaxUses)
            .IsRequired();
            
        builder.Property(il => il.UsedCount)
            .IsRequired();
            
        builder.Property(il => il.CreatedAt)
            .IsRequired();
            
        builder.Property(il => il.ExpiresAt)
            .IsRequired();
            
        // Define relationship with Group
        builder.HasOne(il => il.Group)
            .WithMany()
            .HasForeignKey(il => il.GroupId)
            .OnDelete(DeleteBehavior.Cascade);
            
        // Define relationship with CreatedBy user
        builder.HasOne(il => il.CreatedBy)
            .WithMany()
            .HasForeignKey(il => il.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
