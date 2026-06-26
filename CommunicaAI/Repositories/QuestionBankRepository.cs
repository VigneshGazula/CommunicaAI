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
            var normalizedRole = role.Trim();
            var normalizedDifficulty = difficulty.Trim();
            
            return await _context.QuestionBanks
                .Where(q => q.Role.ToLower() == normalizedRole.ToLower() && 
                           q.Difficulty.ToLower() == normalizedDifficulty.ToLower())
                .ToListAsync();
        }

        public async Task<List<QuestionBank>> GetByRoleDifficultyAndCategoryAsync(string role, string difficulty, string category)
        {
            var normalizedRole = role.Trim();
            var normalizedDifficulty = difficulty.Trim();
            var normalizedCategory = category.Trim();
            
            return await _context.QuestionBanks
                .Where(q => q.Role.ToLower() == normalizedRole.ToLower() && 
                           q.Difficulty.ToLower() == normalizedDifficulty.ToLower() && 
                           q.Category.ToLower() == normalizedCategory.ToLower())
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

        public async Task<List<string>> GetDistinctRolesAsync()
        {
            return await _context.QuestionBanks
                .Select(q => q.Role)
                .Distinct()
                .OrderBy(r => r)
                .ToListAsync();
        }

        public async Task<List<string>> GetDistinctDifficultiesAsync()
        {
            return await _context.QuestionBanks
                .Select(q => q.Difficulty)
                .Distinct()
                .OrderBy(d => d)
                .ToListAsync();
        }

        public async Task<List<string>> GetDistinctCategoriesAsync()
        {
            return await _context.QuestionBanks
                .Select(q => q.Category)
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync();
        }
    }
}
