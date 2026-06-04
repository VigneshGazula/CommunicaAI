using CommunicaAI.Data;
using CommunicaAI.Models;
using CommunicaAI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CommunicaAI.Repositories
{
    public class InterviewResultRepository : IInterviewResultRepository
    {
        private readonly ApplicationDbContext _context;

        public InterviewResultRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<InterviewResult> CreateAsync(InterviewResult result)
        {
            _context.InterviewResults.Add(result);
            await _context.SaveChangesAsync();
            return result;
        }

        public async Task<InterviewResult?> GetBySessionIdAsync(Guid sessionId)
        {
            return await _context.InterviewResults
                .FirstOrDefaultAsync(r => r.InterviewSessionId == sessionId);
        }
    }
}
