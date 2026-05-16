using CommunicaAI.Models;

namespace CommunicaAI.Services
{
    public interface ITokenService
    {
        (string Token, DateTime ExpiresAtUtc) CreateToken(AppUser user);
    }
}
