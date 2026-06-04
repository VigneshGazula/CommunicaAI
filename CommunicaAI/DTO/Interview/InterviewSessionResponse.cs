namespace CommunicaAI.DTO.Interview
{
    public class InterviewSessionResponse
    {
        public Guid SessionId { get; set; }

        public string Role { get; set; } = string.Empty;

        public string Topic { get; set; } = string.Empty;

        public string Difficulty { get; set; } = string.Empty;

        public int QuestionCount { get; set; }

        public int DurationMinutes { get; set; }

        public string Status { get; set; } = string.Empty;

        public DateTime StartedAt { get; set; }

        public DateTime? CompletedAt { get; set; }
    }
}
