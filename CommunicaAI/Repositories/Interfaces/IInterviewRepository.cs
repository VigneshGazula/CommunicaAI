using CommunicaAI.Models;

namespace CommunicaAI.Repositories.Interfaces
{
    public interface IInterviewRepository
    {
        Task<InterviewSession> CreateAsync(InterviewSession session);
        Task<InterviewSession?> GetByIdAsync(Guid sessionId);
        Task<List<InterviewSession>> GetByUserIdAsync(Guid userId);
        Task UpdateAsync(InterviewSession session);
    }
}
