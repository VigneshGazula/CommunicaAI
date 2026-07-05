using CommunicaAI.Models;

namespace CommunicaAI.Repositories.Interfaces
{
    public interface ICompanyProfileRepository
    {
        Task<List<CompanyProfile>> GetAllActiveAsync();
        Task<CompanyProfile?> GetByIdAsync(Guid id);
    }
}
