namespace CommunicaAI.DTO.Interview
{
    public class CreateInterviewResponse
    {
        public Guid SessionId { get; set; }

        public string Status { get; set; } = string.Empty;

        public DateTime StartedAt { get; set; }
    }
}
