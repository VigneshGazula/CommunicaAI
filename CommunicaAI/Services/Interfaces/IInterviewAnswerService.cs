using CommunicaAI.DTO.Interview;

namespace CommunicaAI.Services.Interfaces
{
    public interface IInterviewAnswerService
    {
        Task<AnswerResponse> SubmitAnswerAsync(Guid sessionId, Guid userId, AnswerSubmitRequest request);
    }
}
