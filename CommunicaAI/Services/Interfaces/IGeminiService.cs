using CommunicaAI.DTOs.Evaluation;

namespace CommunicaAI.Services.Interfaces
{
    public interface IGeminiService
    {
        Task<GeminiEvaluationResponse> EvaluateAnswerAsync(
        string question,
        string answer);
    }
}
