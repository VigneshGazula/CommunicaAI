using System.ComponentModel.DataAnnotations;

namespace CommunicaAI.Models;

public class UserVerificationProfile
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid UserId { get; set; }

    public AppUser User { get; set; } = default!;

    public string EnrollmentAudioUrl { get; set; } = string.Empty;
    public string EnrollmentAudioPublicId { get; set; } = string.Empty;

    public string EnrollmentVideoUrl { get; set; } = string.Empty;
    public string EnrollmentVideoPublicId { get; set; } = string.Empty;

    public DateTime EnrolledAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;
}