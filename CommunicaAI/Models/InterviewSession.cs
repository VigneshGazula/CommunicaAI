namespace CommunicaAI.Models
{
    public class InterviewSession
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string Role { get; set; } = string.Empty;

        public string Topic { get; set; } = string.Empty;

        public string Difficulty { get; set; } = string.Empty;

        public int QuestionCount { get; set; }

        public int DurationMinutes { get; set; }

        public DateTime StartedAt { get; set; }

        public DateTime? CompletedAt { get; set; }

        public string Status { get; set; } = "InProgress";

        // Module 6: Company Intelligence
        public Guid? CompanyProfileId { get; set; }

        // Module 7: Resume Intelligence
        public Guid? ResumeProfileId { get; set; }
    }
}
