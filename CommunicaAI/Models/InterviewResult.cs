using System.ComponentModel.DataAnnotations;

namespace CommunicaAI.Models
{
    public class InterviewResult
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid InterviewSessionId { get; set; }

        public int TotalQuestions { get; set; }

        public int AnsweredQuestions { get; set; }

        public double CompletionPercentage { get; set; }

        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;

        public InterviewSession? InterviewSession { get; set; }
    }
}
