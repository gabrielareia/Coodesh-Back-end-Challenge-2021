using CoodeshPharmaIncAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoodeshPharmaIncAPI.Database.Configurations
{
    public class ContactConfiguration : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder
                .Property(t => t.Cellphone)
                .HasColumnType("nvarchar(30)");

            builder
                .Property(t => t.Email)
                .HasColumnType("nvarchar(50)");

            builder
                .Property(t => t.Phone)
                .HasColumnType("nvarchar(30)");
        }
    }
}
