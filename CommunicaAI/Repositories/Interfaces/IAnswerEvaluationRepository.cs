using CommunicaAI.Models;

public interface IAnswerEvaluationRepository
{
    Task<AnswerEvaluation?> GetByAnswerIdAsync(
        Guid answerId);

    Task<List<AnswerEvaluation>> GetBySessionIdAsync(
        Guid sessionId);

    Task<AnswerEvaluation> CreateAsync(
        AnswerEvaluation evaluation);
    
    Task DeleteAsync(Guid id);
}