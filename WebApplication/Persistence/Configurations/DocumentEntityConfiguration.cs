
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Entities;

namespace Persistence.Configurations;

public class DocumentEntityConfiguration: IEntityTypeConfiguration<DocumentEntity>
{
    public void Configure(EntityTypeBuilder<DocumentEntity> builder)
    {
        builder.HasKey(d => d.DocumentId);

        builder.HasOne(d => d.Author)
            .WithMany(u => u.Documents);
        
        builder.HasMany(d => d.Permissions)
            .WithOne(dp => dp.Document)
            .HasForeignKey(dp => dp.DocumentId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(d => d.AccessType)
            .WithMany(at => at.Documents);
    }
}