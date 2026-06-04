using CommunicaAI.Data;
using CommunicaAI.Models;
using CommunicaAI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CommunicaAI.Repositories
{
    public class InterviewQuestionRepository : IInterviewQuestionRepository
    {
        private readonly ApplicationDbContext _context;

        public InterviewQuestionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<InterviewQuestion> CreateAsync(InterviewQuestion question)
        {
            _context.InterviewQuestions.Add(question);
            await _context.SaveChangesAsync();
            return question;
        }

        public async Task<List<InterviewQuestion>> CreateRangeAsync(List<InterviewQuestion> questions)
        {
            _context.InterviewQuestions.AddRange(questions);
            await _context.SaveChangesAsync();
            return questions;
        }

        public async Task<InterviewQuestion?> GetByIdAsync(Guid id)
        {
            return await _context.InterviewQuestions.FindAsync(id);
        }

        public async Task<List<InterviewQuestion>> GetBySessionIdAsync(Guid sessionId)
        {
            return await _context.InterviewQuestions
                .Where(q => q.InterviewSessionId == sessionId)
                .OrderBy(q => q.OrderNumber)
                .ToListAsync();
        }

        public async Task<InterviewQuestion?> GetBySessionAndQuestionIdAsync(Guid sessionId, Guid questionId)
        {
            return await _context.InterviewQuestions
                .FirstOrDefaultAsync(q => q.InterviewSessionId == sessionId && q.Id == questionId);
        }

        public async Task UpdateAsync(InterviewQuestion question)
        {
            _context.InterviewQuestions.Update(question);
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetAnsweredCountAsync(Guid sessionId)
        {
            return await _context.InterviewQuestions
                .Where(q => q.InterviewSessionId == sessionId && q.IsAnswered)
                .CountAsync();
        }
    }
}
