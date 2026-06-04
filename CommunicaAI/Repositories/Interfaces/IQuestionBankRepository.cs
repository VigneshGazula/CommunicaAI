using CommunicaAI.Models;

namespace CommunicaAI.Repositories.Interfaces
{
    public interface IQuestionBankRepository
    {
        Task<QuestionBank> CreateAsync(QuestionBank question);
        Task<QuestionBank?> GetByIdAsync(Guid id);
        Task<List<QuestionBank>> GetAllAsync();
        Task<List<QuestionBank>> GetByRoleAndDifficultyAsync(string role, string difficulty);
        Task<List<QuestionBank>> GetByRoleDifficultyAndCategoryAsync(string role, string difficulty, string category);
        Task UpdateAsync(QuestionBank question);
        Task DeleteAsync(Guid id);
    }
}
