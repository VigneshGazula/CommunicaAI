using CommunicaAI.DTO.Interview;

namespace CommunicaAI.Services.Interfaces
{
    public interface IInterviewAnswerService
    {
        Task<AnswerResponse> SubmitAnswerAsync(Guid sessionId, Guid userId, AnswerSubmitRequest request);
        Task<SubmitAudioAnswerResponse>
SubmitAudioAnswerAsync(
    Guid sessionId,
    Guid questionId,
    IFormFile audioFile,
    int durationSeconds,
    Guid userId);
    }
}
