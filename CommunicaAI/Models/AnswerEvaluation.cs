namespace CommunicaAI.Models
{
    public class AnswerEvaluation
    {
        public Guid Id { get; set; }

        public Guid InterviewAnswerId { get; set; }

        // Technical Evaluation
        public int TechnicalScore { get; set; }

        public int ClarityScore { get; set; }

        public int CompletenessScore { get; set; }

        public int OverallScore { get; set; }

        // AI Communication Evaluation (Module 3)
        public int CommunicationScore { get; set; }

        public int ConfidenceScore { get; set; }

        public int GrammarScore { get; set; }

        public int VocabularyScore { get; set; }

        public int ProfessionalismScore { get; set; }

        public int AnswerStructureScore { get; set; }

        public int PersuasivenessScore { get; set; }

        public int ConcisenessScore { get; set; }

        public string Strengths { get; set; } = string.Empty;

        public string Improvements { get; set; } = string.Empty;

        public string Feedback { get; set; } = string.Empty;

        public DateTime EvaluatedAt { get; set; }

        public InterviewAnswer InterviewAnswer { get; set; } = null!;
    }
}
