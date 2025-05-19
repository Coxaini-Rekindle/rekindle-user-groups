using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rekindle.UserGroups.Domain.Entities.GroupUsers;

namespace Rekindle.UserGroups.DataAccess.Configurations;

public class GroupUserEntityTypeConfiguration : IEntityTypeConfiguration<GroupUser>
{
    public void Configure(EntityTypeBuilder<GroupUser> builder)
    {
        builder.HasKey(gu => new { gu.UserId, gu.GroupId });
        
        builder.Property(gu => gu.Role)
            .IsRequired();
            
        builder.Property(gu => gu.JoinedAt)
            .IsRequired();
            
        // Define relationship with User
        builder.HasOne(gu => gu.User)
            .WithMany()
            .HasForeignKey(gu => gu.UserId)
            .OnDelete(DeleteBehavior.Cascade);
            
        // The relationship with Group is defined in GroupEntityTypeConfiguration
    }
}
