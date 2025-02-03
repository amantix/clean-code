using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebAppMarkdown.Db.Models;

namespace WebAppMarkdown.Db.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Username).IsRequired().HasMaxLength(100);
        builder.HasMany(u => u.MarkdownEntries)
            .WithOne(m => m.User)
            .HasForeignKey(m => m.UserId);
    }
}