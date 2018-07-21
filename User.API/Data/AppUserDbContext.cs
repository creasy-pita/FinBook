using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User.API.Model;


namespace User.API.Data
{
    public class AppUserDbContext : DbContext
    {
        public DbSet<AppUser> Users { get; set; }
        public DbSet<UserTag> UserTags { get; set; }

        private ILogger<AppUserDbContext> _logger;

        public AppUserDbContext(DbContextOptions<AppUserDbContext> options, ILogger<AppUserDbContext> logger) : base(options)
        {
            _logger = logger;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUser>(entity =>
            {
                entity.ToTable("Users").HasKey(e => e.Id);
            });

            modelBuilder.Entity<UserProperty>().Property(u => u.Key).HasMaxLength(100);
            modelBuilder.Entity<UserProperty>().Property(u => u.Value).HasMaxLength(100);
            modelBuilder.Entity<UserProperty>()
                .ToTable("UserProperties")
                .HasKey(u => new { u.Key, u.AppUserId, u.Value });

            modelBuilder.Entity<UserTag>().Property(u => u.Tag).HasMaxLength(100);
            modelBuilder.Entity<UserTag>()
                .ToTable("UserTags")
                .HasKey(u => new { u.UserId, u.Tag});

            modelBuilder.Entity<BPFile>()
                .ToTable("UserBPFiles")
                .HasKey(u => new { u.Id });

            base.OnModelCreating(modelBuilder);


        }


    }


}
