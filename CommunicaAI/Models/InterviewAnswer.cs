using System.ComponentModel.DataAnnotations;

namespace CommunicaAI.Models
{
    public class InterviewAnswer
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid InterviewQuestionId { get; set; }

        [Required]
        public Guid InterviewSessionId { get; set; }

        [Required]
        public string Transcript { get; set; } = string.Empty;

        public DateTime AnsweredAt { get; set; } = DateTime.UtcNow;

        public InterviewQuestion? InterviewQuestion { get; set; }
        public InterviewSession? InterviewSession { get; set; }
    }
}
