using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Entities;

namespace Persistence.Configurations;

public class DocumentAccessConfiguration : IEntityTypeConfiguration<DocumentAccessEntity>
{
    public void Configure(EntityTypeBuilder<DocumentAccessEntity> builder)
    {
        builder.HasKey(access => access.Id);
        
        builder.Property(docAccess => docAccess.Role)
            .HasConversion<string>()
            .IsRequired();
        
        builder
            .HasOne(access => access.User)
            .WithMany(user => user.DocumentAccesses)
            .HasForeignKey(access => access.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder
            .HasOne(access => access.Document)
            .WithMany(document => document.DocumentAccesses)
            .HasForeignKey(access => access.DocumentId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasIndex(access => new { access.UserId, access.DocumentId }).IsUnique();
    }
}
