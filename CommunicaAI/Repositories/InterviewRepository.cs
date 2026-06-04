using CommunicaAI.Data;
using CommunicaAI.Models;
using CommunicaAI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CommunicaAI.Repositories
{
    public class InterviewRepository : IInterviewRepository
    {
        private readonly ApplicationDbContext _context;

        public InterviewRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<InterviewSession> CreateAsync(InterviewSession session)
        {
            _context.InterviewSessions.Add(session);
            await _context.SaveChangesAsync();
            return session;
        }

        public async Task<InterviewSession?> GetByIdAsync(Guid sessionId)
        {
            return await _context.InterviewSessions
                .FirstOrDefaultAsync(x => x.Id == sessionId);
        }

        public async Task<List<InterviewSession>> GetByUserIdAsync(Guid userId)
        {
            return await _context.InterviewSessions
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.StartedAt)
                .ToListAsync();
        }

        public async Task UpdateAsync(InterviewSession session)
        {
            _context.InterviewSessions.Update(session);
            await _context.SaveChangesAsync();
        }
    }
}
