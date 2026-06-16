public class InterviewResult
{
    public Guid Id { get; set; }

    public Guid InterviewSessionId { get; set; }

    public int OverallScore { get; set; }

    public int TechnicalScore { get; set; }

    public int CommunicationScore { get; set; }

    public int ConfidenceScore { get; set; }

    public string Strengths { get; set; } = string.Empty;

    public string Weaknesses { get; set; } = string.Empty;

    public string Recommendations { get; set; } = string.Empty;

    public string Summary { get; set; } = string.Empty;

    public DateTime GeneratedAt { get; set; }
}