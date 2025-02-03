using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebAppMarkdown.Db.Models;

namespace WebAppMarkdown.Db.Configuration;

public class DocumentConfiguration : IEntityTypeConfiguration<DocumentEntity>
{
    public void Configure(EntityTypeBuilder<DocumentEntity> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(m => m.MarkdownContent).IsRequired();
        builder.Property(m => m.HtmlContent);
        builder.Property(m => m.CreatedAt).IsRequired();
    }
}
