using MarkDown.DataBase.models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarkDown.DataBase.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<Users>
    {
        public void Configure(EntityTypeBuilder<Users> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasMany(x => x.Documents)
                .WithOne(x => x.Users)
                .HasForeignKey(x => x.UserId);
        }
    }
}
