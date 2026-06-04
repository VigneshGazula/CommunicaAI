using CommunicaAI.DTO.Interview;

namespace CommunicaAI.Services.Interfaces
{
    public interface IInterviewService
    {
        Task<CreateInterviewResponse> CreateInterviewAsync(Guid userId, CreateInterviewRequest request);
        Task<InterviewSessionResponse?> GetInterviewAsync(Guid sessionId, Guid userId);
        Task<List<InterviewSessionResponse>> GetUserInterviewsAsync(Guid userId);
        Task<bool> CompleteInterviewAsync(Guid sessionId, Guid userId);
    }
}
