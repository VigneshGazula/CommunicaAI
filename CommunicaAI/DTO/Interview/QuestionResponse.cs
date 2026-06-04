namespace CommunicaAI.DTO.Interview
{
    public class QuestionResponse
    {
        public Guid Id { get; set; }
        public int OrderNumber { get; set; }
        public string Category { get; set; } = string.Empty;
        public string QuestionText { get; set; } = string.Empty;
        public bool IsAnswered { get; set; }
    }
}
