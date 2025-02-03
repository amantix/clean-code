using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Entities;

namespace Persistence.Configurations;

public class DocumentPermissionEntityConfiguration: IEntityTypeConfiguration<DocumentPermissionEntity>
{
    public void Configure(EntityTypeBuilder<DocumentPermissionEntity> builder)
    {
        builder.HasKey(dp => dp.DocumentPermissionId);
        
        builder
            .HasOne(dp => dp.Document)
            .WithMany(d => d.Permissions);

        builder
            .HasOne(dp => dp.AccessLevel)
            .WithMany(al => al.DocumentPermissions);

        builder.HasOne(dp => dp.User)
            .WithMany(u => u.DocumentsPermissions);
    }
}