namespace CommunicaAI.DTO.Interview
{
    public class CreateInterviewRequest
    {
        public string Role { get; set; } = string.Empty;

        public string Topic { get; set; } = string.Empty;

        public string Difficulty { get; set; } = string.Empty;

        public int QuestionCount { get; set; }

        public int DurationMinutes { get; set; }
    }
}
