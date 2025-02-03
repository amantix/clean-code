using Microsoft.EntityFrameworkCore;
using WebAppMarkdown.Db.Configuration;
using WebAppMarkdown.Db.Models;

namespace WebAppMarkdown.Db;

public class ApplicationDbContext : DbContext
{
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<DocumentEntity> MarkdownEntries { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new DocumentConfiguration());
    }
}