using CoodeshPharmaIncAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoodeshPharmaIncAPI.Database.Configurations
{
    public class LoginConfiguration : IEntityTypeConfiguration<Login>
    {
        public void Configure(EntityTypeBuilder<Login> builder)
        {
            builder
                .Property(t => t.Username)
                .HasColumnType("nvarchar(40)")
                .IsRequired();

            builder
                .Property(t => t.UUID)
                .HasColumnType("nvarchar(50)")
                .IsRequired();
        }
    }
}
