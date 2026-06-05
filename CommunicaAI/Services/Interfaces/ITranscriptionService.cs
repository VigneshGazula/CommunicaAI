namespace CommunicaAI.Services.Interfaces
{
    public interface ITranscriptionService
    {
        Task<string> TranscribeAsync(
            Stream audioStream,
            string contentType);
    }
}
