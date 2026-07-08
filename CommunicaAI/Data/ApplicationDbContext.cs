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
    public DbSet<InterviewSession> InterviewSessions => Set<InterviewSession>();
    public DbSet<QuestionBank> QuestionBanks => Set<QuestionBank>();
    public DbSet<InterviewQuestion> InterviewQuestions => Set<InterviewQuestion>();
    public DbSet<InterviewAnswer> InterviewAnswers => Set<InterviewAnswer>();
    public DbSet<InterviewResult> InterviewResults => Set<InterviewResult>();

    public DbSet<AnswerEvaluation> AnswerEvaluations { get; set; }

    public DbSet<CompanyProfile> CompanyProfiles { get; set; }

    public DbSet<ResumeProfile> ResumeProfiles { get; set; }

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

        modelBuilder.Entity<InterviewSession>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.HasIndex(x => x.UserId);
            entity.Property(x => x.Role).IsRequired().HasMaxLength(100);
            entity.Property(x => x.Topic).IsRequired().HasMaxLength(200);
            entity.Property(x => x.Difficulty).IsRequired().HasMaxLength(50);
            entity.Property(x => x.Status).IsRequired().HasMaxLength(50);
            entity.Property(x => x.StartedAt).IsRequired();
        });

        modelBuilder.Entity<QuestionBank>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.HasIndex(x => new { x.Role, x.Category, x.Difficulty });
            entity.Property(x => x.Role).IsRequired().HasMaxLength(100);
            entity.Property(x => x.Category).IsRequired().HasMaxLength(50);
            entity.Property(x => x.Difficulty).IsRequired().HasMaxLength(50);
            entity.Property(x => x.QuestionText).IsRequired().HasMaxLength(1000);
        });

        modelBuilder.Entity<InterviewQuestion>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.HasIndex(x => x.InterviewSessionId);
            entity.Property(x => x.Category).IsRequired().HasMaxLength(50);
            entity.Property(x => x.QuestionText).IsRequired().HasMaxLength(1000);
            
            entity.HasOne(x => x.InterviewSession)
                  .WithMany()
                  .HasForeignKey(x => x.InterviewSessionId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<InterviewAnswer>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.HasIndex(x => x.InterviewQuestionId).IsUnique();
            entity.HasIndex(x => x.InterviewSessionId);
            entity.Property(x => x.Transcript).IsRequired();

            entity.HasOne(x => x.InterviewQuestion)
                  .WithOne(x => x.Answer)
                  .HasForeignKey<InterviewAnswer>(x => x.InterviewQuestionId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.InterviewSession)
                  .WithMany()
                  .HasForeignKey(x => x.InterviewSessionId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<InterviewResult>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.HasIndex(x => x.InterviewSessionId).IsUnique();

            entity.HasOne(x => x.InterviewSession)
                  .WithOne()
                  .HasForeignKey<InterviewResult>(x => x.InterviewSessionId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CompanyProfile>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.CompanyName).IsRequired().HasMaxLength(200);
            entity.Property(x => x.InterviewStyle).IsRequired();
            entity.Property(x => x.FocusAreas).IsRequired();
            entity.Property(x => x.BehavioralExpectations).IsRequired();
            entity.Property(x => x.TechnicalExpectations).IsRequired();
            entity.Property(x => x.CommunicationExpectations).IsRequired();
        });

        modelBuilder.Entity<ResumeProfile>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.HasIndex(x => x.UserId);
            entity.Property(x => x.FileName).IsRequired().HasMaxLength(255);
            entity.Property(x => x.ResumeUrl).IsRequired().HasMaxLength(500);
            entity.Property(x => x.FileType).IsRequired().HasMaxLength(50);
        });
    }
}