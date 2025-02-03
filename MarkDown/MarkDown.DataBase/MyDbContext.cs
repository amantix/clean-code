using MarkDown.DataBase.Configurations;
using MarkDown.DataBase.models;
using Microsoft.EntityFrameworkCore;

namespace MarkDown.DataBase
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> dbContextOptions) : base(dbContextOptions) 
        {
        }

        public DbSet<Users> Users { get; set; }

        public DbSet<Documents> Documents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new DocumentConfigration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
