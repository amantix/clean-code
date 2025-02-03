using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Entities;

namespace Persistence.Configurations;

public class DocumentAccessLevelConfiguration: IEntityTypeConfiguration<DocumentAccessLevelEntity>
{
    public void Configure(EntityTypeBuilder<DocumentAccessLevelEntity> builder)
    {
        builder.HasKey(al => al.DocumentAccessLevelId);
        
        builder.HasMany(al => al.DocumentPermissions)
            .WithOne(dp => dp.AccessLevel)
            .HasForeignKey(dp => dp.AccessLevelId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasData(
            Enum.GetValues(typeof(AccessLevel))
                .Cast<AccessLevel>()
                .Select(e => new DocumentAccessLevelEntity
                {
                    DocumentAccessLevelId = (int)e,
                    LevelName = e.ToString()
                })
            );
    }
}