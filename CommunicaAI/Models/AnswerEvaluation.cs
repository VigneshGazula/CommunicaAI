namespace CommunicaAI.Models
{
    public class AnswerEvaluation
    {
        public Guid Id { get; set; }

        public Guid InterviewAnswerId { get; set; }

        public int TechnicalScore { get; set; }

        public int ClarityScore { get; set; }

        public int CompletenessScore { get; set; }

        public int OverallScore { get; set; }

        public string Strengths { get; set; } = string.Empty;

        public string Improvements { get; set; } = string.Empty;

        public string Feedback { get; set; } = string.Empty;

        public DateTime EvaluatedAt { get; set; }

        public InterviewAnswer InterviewAnswer { get; set; } = null!;
    }
}
