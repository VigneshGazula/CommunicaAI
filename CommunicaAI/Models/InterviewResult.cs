namespace CommunicaAI.Models;

public class InterviewResult
{
    public Guid Id { get; set; }

    public Guid InterviewSessionId { get; set; }

    // Statistics properties
    public int TotalQuestions { get; set; }
    
    public int AnsweredQuestions { get; set; }
    
    public double CompletionPercentage { get; set; }

    // Score properties
    public int OverallScore { get; set; }

    public int TechnicalScore { get; set; }

    public int CommunicationScore { get; set; }

    public int ConfidenceScore { get; set; }

    // Feedback properties
    public string Strengths { get; set; } = string.Empty;

    public string Weaknesses { get; set; } = string.Empty;

    public string Recommendations { get; set; } = string.Empty;

    public string Summary { get; set; } = string.Empty;

    public DateTime GeneratedAt { get; set; }

    // Navigation property
    public InterviewSession? InterviewSession { get; set; }
}