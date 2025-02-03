using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Entities;

namespace Persistence.Configurations;

public class DocumentConfiguration : IEntityTypeConfiguration<DocumentEntity>
{
    public void Configure(EntityTypeBuilder<DocumentEntity> builder)
    {
        builder.HasKey(document => document.Id);
        
        builder.Property(document => document.Name)
            .IsRequired()
            .HasMaxLength(255);
        
        builder.Property(document => document.AuthorName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(document => document.CreationDateTime)
            .IsRequired();

        builder
            .HasOne(document => document.Author)
            .WithMany(user => user.PersonalDocuments)
            .HasForeignKey(document => document.AuthorId);
    }
}