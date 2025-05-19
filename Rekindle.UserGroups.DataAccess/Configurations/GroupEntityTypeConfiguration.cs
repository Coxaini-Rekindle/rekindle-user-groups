using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rekindle.UserGroups.Domain.Entities.Groups;

namespace Rekindle.UserGroups.DataAccess.Configurations;

public class GroupEntityTypeConfiguration : IEntityTypeConfiguration<Group>
{
    public void Configure(EntityTypeBuilder<Group> builder)
    {
        builder.HasKey(g => g.Id);
        
        builder.Property(g => g.Name)
            .IsRequired()
            .HasMaxLength(100);
            
        builder.Property(g => g.Description)
            .IsRequired()
            .HasMaxLength(500);
            
        builder.Property(g => g.CreatedAt)
            .IsRequired();
            
        // Define the relationship with GroupUser
        builder.HasMany(g => g.Members)
            .WithOne(gu => gu.Group)
            .HasForeignKey(gu => gu.GroupId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
