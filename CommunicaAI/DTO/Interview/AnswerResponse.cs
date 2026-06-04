namespace CommunicaAI.DTO.Interview
{
    public class AnswerResponse
    {
        public Guid Id { get; set; }
        public Guid QuestionId { get; set; }
        public string Transcript { get; set; } = string.Empty;
        public DateTime AnsweredAt { get; set; }
    }
}
