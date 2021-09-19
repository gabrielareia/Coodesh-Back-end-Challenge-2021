using CoodeshPharmaIncAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoodeshPharmaIncAPI.Database.Configurations
{
    public class TimeZoneConfiguration : IEntityTypeConfiguration<UserTimeZone>
    {
        public void Configure(EntityTypeBuilder<UserTimeZone> builder)
        {
            builder
                .Property(t => t.Description)
                .HasColumnType("nvarchar(80)");

            builder
                .Property(t => t.Offset)
                .HasColumnType("nvarchar(12)");
        }
    }
}
