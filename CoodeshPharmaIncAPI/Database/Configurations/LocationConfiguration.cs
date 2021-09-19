using CoodeshPharmaIncAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoodeshPharmaIncAPI.Database.Configurations
{
    public class LocationConfiguration : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder
                .Property(t => t.City)
                .HasColumnType("nvarchar(50)");

            builder
                .Property(t => t.Country)
                .HasColumnType("nvarchar(50)");

            builder
                .Property(t => t.Number)
                .HasColumnType("nvarchar(10)");

            builder
                .Property(t => t.PostCode)
                .HasColumnType("nvarchar(20)");

            builder
                .Property(t => t.State)
                .HasColumnType("nvarchar(40)");

            builder
                .Property(t => t.Street)
                .HasColumnType("nvarchar(80)");

            builder
                .HasOne(t => t.TimeZone)
                .WithMany()
                .HasForeignKey("TimeZoneId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
