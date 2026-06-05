public class SubmitAudioAnswerResponse
{
    public Guid AnswerId { get; set; }

    public string Transcript { get; set; } = string.Empty;

    public string AudioUrl { get; set; } = string.Empty;

    public int TechnicalScore { get; set; }

    public int ClarityScore { get; set; }

    public int CompletenessScore { get; set; }

    public int OverallScore { get; set; }

    public string Feedback { get; set; } = string.Empty;
}