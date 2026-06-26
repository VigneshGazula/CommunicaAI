using CommunicaAI.Models;

public interface IAnswerEvaluationRepository
{
    Task<AnswerEvaluation?> GetByAnswerIdAsync(
        Guid answerId);

    Task<AnswerEvaluation> CreateAsync(
        AnswerEvaluation evaluation);
    
    Task DeleteAsync(Guid id);
}