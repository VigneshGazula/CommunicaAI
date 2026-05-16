namespace CommunicaAI.DTO.Media
{
    public class MediaOnboardingResponse
    {
        public Guid UserId { get; set; }
        public string? AudioFilePath { get; set; }
        public string? VideoFilePath { get; set; }
        public bool IsCompleted { get; set; }
    }
}
