using System.ComponentModel.DataAnnotations;

namespace CommunicaAI.Models
{
    public class InterviewQuestion
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid InterviewSessionId { get; set; }

        public int OrderNumber { get; set; }

        [Required]
        [MaxLength(50)]
        public string Category { get; set; } = string.Empty;

        [Required]
        [MaxLength(1000)]
        public string QuestionText { get; set; } = string.Empty;

        public bool IsAnswered { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public InterviewSession? InterviewSession { get; set; }
        public InterviewAnswer? Answer { get; set; }
    }
}
