using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MoneyTrack.Areas.Identity.Data;
using MoneyTrack.Models;

namespace MoneyTrack.Data
{
    public class MoneyTrackContext : IdentityDbContext<MoneyTrackUser>
    {
        public MoneyTrackContext(DbContextOptions<MoneyTrackContext> options)
            : base(options)
        {
        }

        public DbSet<User> UserTable { get; set; }
        public DbSet<Project> ProjectTable { get; set; }
        public DbSet<Investment> Investments { get; set; } // Add this line

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
