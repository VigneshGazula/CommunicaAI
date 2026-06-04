using CommunicaAI.DTO.Interview;

namespace CommunicaAI.Services.Interfaces
{
    public interface IInterviewResultService
    {
        Task<InterviewResultResponse> GenerateResultAsync(Guid sessionId);
    }
}
