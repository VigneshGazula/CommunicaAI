namespace CommunicaAI.DTO.Interview
{
    public class InterviewHistoryResponse
    {
        public Guid SessionId { get; set; }
        public string Role { get; set; } = string.Empty;
        public string Difficulty { get; set; } = string.Empty;
        public DateTime StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string Status { get; set; } = string.Empty;
        public double? CompletionPercentage { get; set; }
        // Module 9: Specialized Interview Modes
        public string InterviewType { get; set; } = "Technical";
    }
}
