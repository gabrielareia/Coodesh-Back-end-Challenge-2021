using CoodeshPharmaIncAPI.Models;
using CoodeshPharmaIncAPI.Models.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace CoodeshPharmaIncAPI.Database.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .Property(t => t.Gender)
                .HasColumnType("nvarchar(10)");

            builder
                .Property(t => t.Birthday)
                .HasConversion(t => (DateTime)t, t => t)
                .HasColumnType("date");
          
            builder
                .Property(t => t.Registered)
                .HasConversion(t => (DateTime)t, t => t)
                .HasColumnType("datetime");
          
            builder
                .Property(t => t.Nationality)
                .HasColumnType("nvarchar(2)");

            builder
                .HasOne(t => t.Name)
                .WithOne()
                .HasForeignKey<User>("NameId")
                .OnDelete(DeleteBehavior.SetNull);

            builder
                .HasOne(t => t.Contact)
                .WithOne()
                .HasForeignKey<User>("ContactId")
                .OnDelete(DeleteBehavior.SetNull);

            builder
                .HasOne(t => t.Login)
                .WithOne()
                .HasForeignKey<User>("LoginId")
                .OnDelete(DeleteBehavior.SetNull);

            builder
                .HasOne(t => t.Location)
                .WithOne()
                .HasForeignKey<User>("LocationId")
                .OnDelete(DeleteBehavior.SetNull);

            builder
                .HasOne(t => t.Picture)                
                .WithOne()
                .HasForeignKey<User>("PictureId")
                .OnDelete(DeleteBehavior.SetNull);

            builder
                .Property(t => t.Imported_T)
                .HasColumnName("imported_t")
                .HasDefaultValueSql("GETDATE()");

            builder
                .Property(t => t.Status)
                .HasColumnName("status")
                .HasConversion(t => t.GetName(), t => t.GetStatus());            
            
        }
    }
}
