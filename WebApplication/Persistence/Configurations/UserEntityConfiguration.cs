

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Entities;

namespace Persistence.Configurations;

public class UserEntityConfiguration: IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.HasKey(u => u.UserId);

        builder
            .HasMany(u => u.Documents)
            .WithOne(d => d.Author)
            .HasForeignKey(d => d.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder
            .HasMany(u => u.DocumentsPermissions)
            .WithOne(dp => dp.User)
            .HasForeignKey(dp => dp.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}