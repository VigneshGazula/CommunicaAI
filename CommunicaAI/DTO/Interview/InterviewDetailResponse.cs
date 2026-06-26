namespace CommunicaAI.DTO.Interview
{
    public class InterviewDetailResponse
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
        public List<QuestionWithAnswerResponse> Questions { get; set; } = new();
        public InterviewResultResponse? Result { get; set; }
    }

    public class QuestionWithAnswerResponse
    {
        public Guid Id { get; set; }
        public int OrderNumber { get; set; }
        public string Category { get; set; } = string.Empty;
        public string QuestionText { get; set; } = string.Empty;
        public bool IsAnswered { get; set; }
        public AnswerResponse? Answer { get; set; }
    }

    public class InterviewResultResponse
    {
        public int TotalQuestions { get; set; }
        public int AnsweredQuestions { get; set; }
        public double CompletionPercentage { get; set; }
        public DateTime GeneratedAt { get; set; }
        public int OverallScore { get; set; }
        public int TechnicalScore { get; set; }
        public int CommunicationScore { get; set; }
        public int ConfidenceScore { get; set; }
        public string Strengths { get; set; } = string.Empty;
        public string Weaknesses { get; set; } = string.Empty;
        public string Recommendations { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
    }
}
