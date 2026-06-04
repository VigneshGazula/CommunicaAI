using CommunicaAI.DTO.Interview;
using CommunicaAI.Models;
using CommunicaAI.Repositories.Interfaces;
using CommunicaAI.Services.Interfaces;

namespace CommunicaAI.Services
{
    public class InterviewService : IInterviewService
    {
        private readonly IInterviewRepository _interviewRepository;

        public InterviewService(IInterviewRepository interviewRepository)
        {
            _interviewRepository = interviewRepository;
        }

        public async Task<CreateInterviewResponse> CreateInterviewAsync(Guid userId, CreateInterviewRequest request)
        {
            var session = new InterviewSession
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Role = request.Role,
                Topic = request.Topic,
                Difficulty = request.Difficulty,
                QuestionCount = request.QuestionCount,
                DurationMinutes = request.DurationMinutes,
                StartedAt = DateTime.UtcNow,
                Status = "InProgress"
            };

            var createdSession = await _interviewRepository.CreateAsync(session);

            return new CreateInterviewResponse
            {
                SessionId = createdSession.Id,
                Status = createdSession.Status,
                StartedAt = createdSession.StartedAt
            };
        }

        public async Task<InterviewSessionResponse?> GetInterviewAsync(Guid sessionId, Guid userId)
        {
            var session = await _interviewRepository.GetByIdAsync(sessionId);

            if (session == null || session.UserId != userId)
            {
                return null;
            }

            return MapToResponse(session);
        }

        public async Task<List<InterviewSessionResponse>> GetUserInterviewsAsync(Guid userId)
        {
            var sessions = await _interviewRepository.GetByUserIdAsync(userId);

            return sessions.Select(MapToResponse).ToList();
        }

        public async Task<bool> CompleteInterviewAsync(Guid sessionId, Guid userId)
        {
            var session = await _interviewRepository.GetByIdAsync(sessionId);

            if (session == null || session.UserId != userId)
            {
                return false;
            }

            session.CompletedAt = DateTime.UtcNow;
            session.Status = "Completed";

            await _interviewRepository.UpdateAsync(session);

            return true;
        }

        private static InterviewSessionResponse MapToResponse(InterviewSession session)
        {
            return new InterviewSessionResponse
            {
                SessionId = session.Id,
                Role = session.Role,
                Topic = session.Topic,
                Difficulty = session.Difficulty,
                QuestionCount = session.QuestionCount,
                DurationMinutes = session.DurationMinutes,
                Status = session.Status,
                StartedAt = session.StartedAt,
                CompletedAt = session.CompletedAt
            };
        }
    }
}
