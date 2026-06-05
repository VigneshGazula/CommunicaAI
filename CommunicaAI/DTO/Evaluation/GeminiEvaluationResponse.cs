namespace CommunicaAI.DTOs.Evaluation;

public class GeminiEvaluationResponse
{
    public int TechnicalScore { get; set; }

    public int ClarityScore { get; set; }

    public int CompletenessScore { get; set; }

    public int OverallScore { get; set; }

    public string Strengths { get; set; } = string.Empty;

    public string Improvements { get; set; } = string.Empty;

    public string Feedback { get; set; } = string.Empty;
}