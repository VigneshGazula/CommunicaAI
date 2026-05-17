using System.ComponentModel.DataAnnotations;

namespace CommunicaAI.Models;

public class UserMediaProfile
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid UserId { get; set; }

    public AppUser User { get; set; } = default!;

    public string? AudioUrl { get; set; }
    public string? AudioPublicId { get; set; }
    public string? AudioContentType { get; set; }
    public long? AudioSizeBytes { get; set; }
    public DateTime? AudioUploadedAtUtc { get; set; }

    public string? VideoUrl { get; set; }
    public string? VideoPublicId { get; set; }
    public string? VideoContentType { get; set; }
    public long? VideoSizeBytes { get; set; }
    public DateTime? VideoUploadedAtUtc { get; set; }

    public bool IsCompleted =>
        !string.IsNullOrWhiteSpace(AudioUrl) &&
        !string.IsNullOrWhiteSpace(VideoUrl);

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;
}