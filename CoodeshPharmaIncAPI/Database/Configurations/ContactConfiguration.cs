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
                .HasColumnType("nvarchar(20)");

            builder
                .Property(t => t.Email)
                .HasColumnType("nvarchar(40)");

            builder
                .Property(t => t.Phone)
                .HasColumnType("nvarchar(20)");
        }
    }
}
