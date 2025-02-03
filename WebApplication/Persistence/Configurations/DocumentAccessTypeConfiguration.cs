using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Entities;

namespace Persistence.Configurations;

public class DocumentAccessTypeConfiguration: IEntityTypeConfiguration<DocumentAccessTypeEntity>
{
    public void Configure(EntityTypeBuilder<DocumentAccessTypeEntity> builder)
    {
        builder.HasKey(at => at.DocumentAccessTypeId);
        
        builder.HasMany(at => at.Documents)
            .WithOne(d => d.AccessType)
            .HasForeignKey(d => d.AccessTypeId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasData(
            Enum.GetValues(typeof(AccessType))
                .Cast<AccessType>()
                .Select(e => new DocumentAccessTypeEntity
                {
                    DocumentAccessTypeId = (int)e,
                    TypeName = e.ToString()
                })
            );
    }
}