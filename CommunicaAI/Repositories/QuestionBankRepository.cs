using CommunicaAI.Data;
using CommunicaAI.Models;
using CommunicaAI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CommunicaAI.Repositories
{
    public class QuestionBankRepository : IQuestionBankRepository
    {
        private readonly ApplicationDbContext _context;

        public QuestionBankRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<QuestionBank> CreateAsync(QuestionBank question)
        {
            _context.QuestionBanks.Add(question);
            await _context.SaveChangesAsync();
            return question;
        }

        public async Task<QuestionBank?> GetByIdAsync(Guid id)
        {
            return await _context.QuestionBanks.FindAsync(id);
        }

        public async Task<List<QuestionBank>> GetAllAsync()
        {
            return await _context.QuestionBanks
                .OrderBy(q => q.Role)
                .ThenBy(q => q.Category)
                .ToListAsync();
        }

        public async Task<List<QuestionBank>> GetByRoleAndDifficultyAsync(string role, string difficulty)
        {
            return await _context.QuestionBanks
                .Where(q => q.Role == role && q.Difficulty == difficulty)
                .ToListAsync();
        }

        public async Task<List<QuestionBank>> GetByRoleDifficultyAndCategoryAsync(string role, string difficulty, string category)
        {
            return await _context.QuestionBanks
                .Where(q => q.Role == role && q.Difficulty == difficulty && q.Category == category)
                .ToListAsync();
        }

        public async Task UpdateAsync(QuestionBank question)
        {
            _context.QuestionBanks.Update(question);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var question = await GetByIdAsync(id);
            if (question != null)
            {
                _context.QuestionBanks.Remove(question);
                await _context.SaveChangesAsync();
            }
        }
    }
}
