using CommunicaAI.Models;

namespace CommunicaAI.Repositories.Interfaces
{
    public interface IInterviewAnswerRepository
    {
        Task<InterviewAnswer> CreateAsync(InterviewAnswer answer);
        Task<InterviewAnswer?> GetByQuestionIdAsync(Guid questionId);
        Task<List<InterviewAnswer>> GetBySessionIdAsync(Guid sessionId);
    }
}
