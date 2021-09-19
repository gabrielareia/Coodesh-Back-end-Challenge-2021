using CoodeshPharmaIncAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoodeshPharmaIncAPI.Database.Configurations
{
    public class NameConfiguration : IEntityTypeConfiguration<Name>
    {
        public void Configure(EntityTypeBuilder<Name> builder)
        {
            builder
                .Property(t => t.Title)
                .HasColumnType("nvarchar(5)");

            builder
                .Property(t => t.First)
                .HasColumnType("nvarchar(40)")
                .IsRequired();

            builder
                .Property(t => t.Last)
                .HasColumnType("nvarchar(40)")
                .IsRequired();
        }
    }
}
