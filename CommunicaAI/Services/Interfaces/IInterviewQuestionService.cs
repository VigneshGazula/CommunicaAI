using CommunicaAI.DTO.Interview;

namespace CommunicaAI.Services.Interfaces
{
    public interface IInterviewQuestionService
    {
        Task<List<QuestionResponse>> GetSessionQuestionsAsync(Guid sessionId, Guid userId);
        Task GenerateQuestionsForSessionAsync(Guid sessionId, string role, string difficulty, int questionCount);
    }
}
