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

    // Video Intelligence Scores (Module 4)
    public int EyeContactScore { get; set; }

    public int PostureScore { get; set; }

    public int FacialExpressionScore { get; set; }

    public int VideoConfidenceScore { get; set; }

    public string VideoFeedback { get; set; } = string.Empty;

    // Feedback properties
    public string Strengths { get; set; } = string.Empty;

    public string Weaknesses { get; set; } = string.Empty;

    public string Recommendations { get; set; } = string.Empty;

    public string Summary { get; set; } = string.Empty;

    // AI Interview Coach (Module 5)
    public string CoachingSummary { get; set; } = string.Empty;

    public string CoachingStrengths { get; set; } = string.Empty;

    public string CoachingWeaknesses { get; set; } = string.Empty;

    public string CommunicationImprovements { get; set; } = string.Empty;

    public string TechnicalImprovements { get; set; } = string.Empty;

    public string VideoImprovements { get; set; } = string.Empty;

    public string VoiceImprovements { get; set; } = string.Empty;

    public string PracticeRecommendations { get; set; } = string.Empty;

    public string SuggestedRole { get; set; } = string.Empty;

    public string SuggestedDifficulty { get; set; } = string.Empty;

    public int SuggestedQuestionCount { get; set; }

    public string LearningResources { get; set; } = string.Empty;

    public string MotivationalMessage { get; set; } = string.Empty;

    // Company Intelligence (Module 6)
    public int CompanyReadinessScore { get; set; }

    public int TechnicalAlignment { get; set; }

    public int CommunicationAlignment { get; set; }

    public int CultureFit { get; set; }

    public string CompanySpecificFeedback { get; set; } = string.Empty;

    // Resume Intelligence (Module 7)
    public int ResumeMatchScore { get; set; }

    public string SkillGapSummary { get; set; } = string.Empty;

    public string CareerRecommendations { get; set; } = string.Empty;

    public DateTime GeneratedAt { get; set; }

    // Navigation property
    public InterviewSession? InterviewSession { get; set; }
}
