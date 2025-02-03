using MarkDown.DataBase.models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkDown.DataBase.Configurations
{
    public class DocumentConfigration : IEntityTypeConfiguration<Documents>
    {
        public void Configure(EntityTypeBuilder<Documents> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Users)
                .WithMany(x => x.Documents)
                .HasForeignKey(x => x.UserId);   
        }
    }
}
