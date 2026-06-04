using CommunicaAI.Data;
using CommunicaAI.Models;
using CommunicaAI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CommunicaAI.Repositories
{
    public class InterviewAnswerRepository : IInterviewAnswerRepository
    {
        private readonly ApplicationDbContext _context;

        public InterviewAnswerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<InterviewAnswer> CreateAsync(InterviewAnswer answer)
        {
            _context.InterviewAnswers.Add(answer);
            await _context.SaveChangesAsync();
            return answer;
        }

        public async Task<InterviewAnswer?> GetByQuestionIdAsync(Guid questionId)
        {
            return await _context.InterviewAnswers
                .FirstOrDefaultAsync(a => a.InterviewQuestionId == questionId);
        }

        public async Task<List<InterviewAnswer>> GetBySessionIdAsync(Guid sessionId)
        {
            return await _context.InterviewAnswers
                .Where(a => a.InterviewSessionId == sessionId)
                .OrderBy(a => a.AnsweredAt)
                .ToListAsync();
        }
    }
}
