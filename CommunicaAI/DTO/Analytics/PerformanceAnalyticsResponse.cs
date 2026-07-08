namespace CommunicaAI.DTO.Analytics;

public class PerformanceAnalyticsResponse
{
    public OverallProgressData OverallProgress { get; set; } = new();
    public List<TrendDataPoint> TechnicalScoreTrend { get; set; } = new();
    public List<TrendDataPoint> CommunicationScoreTrend { get; set; } = new();
    public List<TrendDataPoint> ConfidenceScoreTrend { get; set; } = new();
    public List<TrendDataPoint> VideoAnalysisTrend { get; set; } = new();
    public List<TrendDataPoint> ResumeMatchTrend { get; set; } = new();
    public List<TrendDataPoint> CompanyReadinessTrend { get; set; } = new();
    public List<SkillData> StrongestSkills { get; set; } = new();
    public List<SkillData> WeakestSkills { get; set; } = new();
    public PracticeRecommendationsData PracticeRecommendations { get; set; } = new();
    public WeeklyProgressData WeeklyProgress { get; set; } = new();
}

public class OverallProgressData
{
    public int TotalInterviews { get; set; }
    public int CompletedInterviews { get; set; }
    public int AverageOverallScore { get; set; }
    public int AverageTechnicalScore { get; set; }
    public int AverageCommunicationScore { get; set; }
    public int AverageConfidenceScore { get; set; }
    public int CurrentStreak { get; set; }
    public int LongestStreak { get; set; }
    public double ImprovementRate { get; set; } // Percentage improvement over last 5 interviews
}

public class TrendDataPoint
{
    public DateTime Date { get; set; }
    public int Score { get; set; }
    public string Role { get; set; } = string.Empty;
    public string Difficulty { get; set; } = string.Empty;
}

public class SkillData
{
    public string SkillName { get; set; } = string.Empty;
    public int AverageScore { get; set; }
    public int Frequency { get; set; } // How many times evaluated
    public string Category { get; set; } = string.Empty; // Technical, Communication, Video, etc.
}

public class PracticeRecommendationsData
{
    public List<string> FocusAreas { get; set; } = new();
    public string RecommendedRole { get; set; } = string.Empty;
    public string RecommendedDifficulty { get; set; } = string.Empty;
    public List<string> TopicsToImprove { get; set; } = new();
    public string NextStepsSummary { get; set; } = string.Empty;
}

public class WeeklyProgressData
{
    public int InterviewsThisWeek { get; set; }
    public int InterviewsLastWeek { get; set; }
    public int AverageScoreThisWeek { get; set; }
    public int AverageScoreLastWeek { get; set; }
    public int WeekOverWeekImprovement { get; set; } // Percentage change
}
