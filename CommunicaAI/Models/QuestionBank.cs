using System.ComponentModel.DataAnnotations;

namespace CommunicaAI.Models
{
    public class QuestionBank
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(100)]
        public string Role { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Category { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Difficulty { get; set; } = string.Empty;

        [Required]
        [MaxLength(1000)]
        public string QuestionText { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
