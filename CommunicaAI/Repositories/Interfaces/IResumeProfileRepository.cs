using CommunicaAI.Models;

namespace CommunicaAI.Repositories.Interfaces
{
    public interface IResumeProfileRepository
    {
        Task<ResumeProfile> CreateAsync(ResumeProfile profile);
        Task<ResumeProfile?> GetByIdAsync(Guid id);
        Task<ResumeProfile?> GetLatestByUserIdAsync(Guid userId);
        Task<List<ResumeProfile>> GetByUserIdAsync(Guid userId);
    }
}
