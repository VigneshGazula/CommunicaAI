namespace CommunicaAI.Services.Interfaces
{
    public interface IGeminiService
    {
        Task<SubmitAudioAnswerResponse> EvaluateAnswerAsync(
        string question,
        string answer);
    }
}
