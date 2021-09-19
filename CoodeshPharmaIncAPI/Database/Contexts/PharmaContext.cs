using CoodeshPharmaIncAPI.Database.Configurations;
using CoodeshPharmaIncAPI.Models;
using CoodeshPharmaIncAPI.Models.Import;
using Microsoft.EntityFrameworkCore;

namespace CoodeshPharmaIncAPI.Database
{
    public class PharmaContext : DbContext
    {
        //Models
        public DbSet<User> User { get; set; }
        public DbSet<Name> Name { get; set; }
        public DbSet<Login> Login { get; set; }
        public DbSet<Contact> Contact { get; set; }
        public DbSet<Picture> Picture { get; set; }
        public DbSet<Location> Location { get; set; }
        public DbSet<UserTimeZone> TimeZone { get; set; }

        //Import
        public DbSet<ImportInfo> ImportInfo { get; set; }

        public PharmaContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new ContactConfiguration());
            modelBuilder.ApplyConfiguration(new LocationConfiguration());
            modelBuilder.ApplyConfiguration(new LoginConfiguration());
            modelBuilder.ApplyConfiguration(new NameConfiguration());
            modelBuilder.ApplyConfiguration(new TimeZoneConfiguration());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
