using System.ComponentModel.DataAnnotations;

namespace CommunicaAI.Models
{
    public class AppUser
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        public UserMediaProfile? MediaProfile { get; set; }
        public UserVerificationProfile? VerificationProfile { get; set; }
    }
}
