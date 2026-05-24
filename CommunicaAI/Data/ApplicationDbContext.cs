using CommunicaAI.Models;
using Microsoft.EntityFrameworkCore;

namespace CommunicaAI.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<AppUser> Users => Set<AppUser>();
    public DbSet<UserVerificationProfile> UserVerificationProfiles => Set<UserVerificationProfile>();


    public object AppUser { get; internal set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.HasIndex(x => x.Email).IsUnique();
            entity.Property(x => x.FullName).IsRequired().HasMaxLength(100);
            entity.Property(x => x.Email).IsRequired().HasMaxLength(150);
            entity.Property(x => x.PasswordHash).IsRequired();
        });

        modelBuilder.Entity<UserVerificationProfile>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.HasIndex(x => x.UserId).IsUnique();

            entity.HasOne(x => x.User)
                  .WithOne(x => x.VerificationProfile)
                  .HasForeignKey<UserVerificationProfile>(x => x.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}