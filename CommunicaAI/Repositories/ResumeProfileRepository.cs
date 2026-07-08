using CommunicaAI.Data;
using CommunicaAI.Models;
using CommunicaAI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CommunicaAI.Repositories
{
    public class ResumeProfileRepository : IResumeProfileRepository
    {
        private readonly ApplicationDbContext _context;

        public ResumeProfileRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ResumeProfile> CreateAsync(ResumeProfile profile)
        {
            _context.ResumeProfiles.Add(profile);
            await _context.SaveChangesAsync();
            return profile;
        }

        public async Task<ResumeProfile?> GetByIdAsync(Guid id)
        {
            return await _context.ResumeProfiles
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<ResumeProfile?> GetLatestByUserIdAsync(Guid userId)
        {
            return await _context.ResumeProfiles
                .Where(r => r.UserId == userId && r.IsActive)
                .OrderByDescending(r => r.UploadedAt)
                .FirstOrDefaultAsync();
        }

        public async Task<List<ResumeProfile>> GetByUserIdAsync(Guid userId)
        {
            return await _context.ResumeProfiles
                .Where(r => r.UserId == userId && r.IsActive)
                .OrderByDescending(r => r.UploadedAt)
                .ToListAsync();
        }
    }
}
