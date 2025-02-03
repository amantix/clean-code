using Microsoft.EntityFrameworkCore;
using Persistence.Configurations;
using Persistence.Entities;

namespace Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options): DbContext(options)
{
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<DocumentEntity> Documents { get; set; }
    public DbSet<DocumentPermissionEntity> Permissions { get; set; }
    public DbSet<DocumentAccessTypeEntity> AccessTypes { get; set; }
    public DbSet<DocumentAccessLevelEntity> AccessLevels { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
        modelBuilder.ApplyConfiguration(new DocumentEntityConfiguration());
        modelBuilder.ApplyConfiguration(new DocumentPermissionEntityConfiguration());
        modelBuilder.ApplyConfiguration(new DocumentAccessTypeConfiguration());
        modelBuilder.ApplyConfiguration(new DocumentAccessLevelConfiguration());
        
        base.OnModelCreating(modelBuilder);
    }
}