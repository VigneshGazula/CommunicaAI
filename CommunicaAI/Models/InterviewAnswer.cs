using CommunicaAI.Models;

public class InterviewAnswer
{
    public Guid Id { get; set; }

    public Guid InterviewQuestionId { get; set; }

    public Guid InterviewSessionId { get; set; }

    public string Transcript { get; set; } = string.Empty;

    public string? AudioUrl { get; set; }

    public int DurationSeconds { get; set; }

    public DateTime AnsweredAt { get; set; }
    public InterviewQuestion? InterviewQuestion { get; set; }
    public InterviewSession? InterviewSession { get; set; }

    public AnswerEvaluation? Evaluation { get; set; }
}