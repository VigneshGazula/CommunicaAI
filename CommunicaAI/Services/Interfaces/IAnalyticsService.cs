using CommunicaAI.DTO.Analytics;

namespace CommunicaAI.Services.Interfaces;

public interface IAnalyticsService
{
    Task<PerformanceAnalyticsResponse> GetUserPerformanceAnalyticsAsync(Guid userId);
}
