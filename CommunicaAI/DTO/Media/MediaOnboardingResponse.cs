namespace CommunicaAI.DTO.Media
{
    public class MediaOnboardingResponse
    {
        public Guid UserId { get; set; }

        public string? AudioUrl { get; set; }
        public string? AudioPublicId { get; set; }

        public string? VideoUrl { get; set; }
        public string? VideoPublicId { get; set; }

        public bool IsCompleted { get; set; }
    }
}
