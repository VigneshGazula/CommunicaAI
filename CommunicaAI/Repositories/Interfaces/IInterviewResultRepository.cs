using CommunicaAI.Models;

namespace CommunicaAI.Repositories.Interfaces
{
    public interface IInterviewResultRepository
    {
        Task<InterviewResult> CreateAsync(InterviewResult result);
        Task<InterviewResult?> GetBySessionIdAsync(Guid sessionId);
        Task<InterviewResult?> GetByIdAsync(Guid id);
        Task<InterviewResult> UpdateAsync(InterviewResult result);
    }
}
