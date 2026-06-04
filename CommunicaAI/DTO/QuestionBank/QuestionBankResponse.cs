namespace CommunicaAI.DTO.QuestionBank
{
    public class QuestionBankResponse
    {
        public Guid Id { get; set; }
        public string Role { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Difficulty { get; set; } = string.Empty;
        public string QuestionText { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
