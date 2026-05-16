using CommunicaAI.Models;
using Microsoft.EntityFrameworkCore;

namespace CommunicaAI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<UserMediaProfile> UserMediaProfiles => Set<UserMediaProfile>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configure AppUser entity
            modelBuilder.Entity<AppUser>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.FullName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(150);
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.Property(e => e.CreatedAtUtc).IsRequired();
            });

            modelBuilder.Entity<UserMediaProfile>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.HasIndex(x => x.UserId).IsUnique();

                entity.HasOne(x => x.User)
                      .WithOne(x => x.MediaProfile)
                      .HasForeignKey<UserMediaProfile>(x => x.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
