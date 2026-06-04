using CommunicaAI.Models;

namespace CommunicaAI.Repositories.Interfaces
{
    public interface IInterviewQuestionRepository
    {
        Task<InterviewQuestion> CreateAsync(InterviewQuestion question);
        Task<List<InterviewQuestion>> CreateRangeAsync(List<InterviewQuestion> questions);
        Task<InterviewQuestion?> GetByIdAsync(Guid id);
        Task<List<InterviewQuestion>> GetBySessionIdAsync(Guid sessionId);
        Task<InterviewQuestion?> GetBySessionAndQuestionIdAsync(Guid sessionId, Guid questionId);
        Task UpdateAsync(InterviewQuestion question);
        Task<int> GetAnsweredCountAsync(Guid sessionId);
    }
}
