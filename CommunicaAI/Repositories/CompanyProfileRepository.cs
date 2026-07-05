using CommunicaAI.Data;
using CommunicaAI.Models;
using CommunicaAI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CommunicaAI.Repositories
{
    public class CompanyProfileRepository : ICompanyProfileRepository
    {
        private readonly ApplicationDbContext _context;

        public CompanyProfileRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<CompanyProfile>> GetAllActiveAsync()
        {
            return await _context.CompanyProfiles
                .Where(c => c.IsActive)
                .OrderBy(c => c.CompanyName)
                .ToListAsync();
        }

        public async Task<CompanyProfile?> GetByIdAsync(Guid id)
        {
            return await _context.CompanyProfiles
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
