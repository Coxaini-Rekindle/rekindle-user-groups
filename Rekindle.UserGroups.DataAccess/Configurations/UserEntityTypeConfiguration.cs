using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rekindle.UserGroups.Domain.Entities.Users;

namespace Rekindle.UserGroups.DataAccess.Configurations;

public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        
        builder.Property(u => u.Name)
            .IsRequired();
        
        builder.Property(u => u.Username)
            .IsRequired();
            
        builder.HasIndex(u => u.Username)
            .IsUnique();
            
        builder.Property(u => u.Email)
            .IsRequired();
            
        builder.HasIndex(u => u.Email)
            .IsUnique();
            
        builder.Property(u => u.PasswordHash)
            .IsRequired();
            
        builder.Property(u => u.RefreshToken);
        
        builder.Property(u => u.RefreshTokenExpiryTime);
    }
}
