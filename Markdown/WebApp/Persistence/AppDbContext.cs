using Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Configurations;

namespace Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<DocumentEntity> Documents { get; set; }
    public DbSet<DocumentAccessEntity> DocumentAccesses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new DocumentConfiguration());
        modelBuilder.ApplyConfiguration(new DocumentAccessConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}