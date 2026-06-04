using CommunicaAI.DTO.Interview;
using CommunicaAI.Models;
using CommunicaAI.Repositories.Interfaces;
using CommunicaAI.Services.Interfaces;

namespace CommunicaAI.Services
{
    public class InterviewService : IInterviewService
    {
        private readonly IInterviewRepository _interviewRepository;
        private readonly IInterviewQuestionService _questionService;
        private readonly IInterviewResultService _resultService;
        private readonly IInterviewQuestionRepository _questionRepository;
        private readonly IInterviewAnswerRepository _answerRepository;
        private readonly IInterviewResultRepository _resultRepository;

        public InterviewService(
            IInterviewRepository interviewRepository,
            IInterviewQuestionService questionService,
            IInterviewResultService resultService,
            IInterviewQuestionRepository questionRepository,
            IInterviewAnswerRepository answerRepository,
            IInterviewResultRepository resultRepository)
        {
            _interviewRepository = interviewRepository;
            _questionService = questionService;
            _resultService = resultService;
            _questionRepository = questionRepository;
            _answerRepository = answerRepository;
            _resultRepository = resultRepository;
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

            await _questionService.GenerateQuestionsForSessionAsync(
                createdSession.Id,
                request.Role,
                request.Difficulty,
                request.QuestionCount
            );

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

            await _resultService.GenerateResultAsync(sessionId);

            return true;
        }

        public async Task<List<InterviewHistoryResponse>> GetUserHistoryAsync(Guid userId)
        {
            var sessions = await _interviewRepository.GetByUserIdAsync(userId);
            var historyList = new List<InterviewHistoryResponse>();

            foreach (var session in sessions)
            {
                var result = await _resultRepository.GetBySessionIdAsync(session.Id);
                
                historyList.Add(new InterviewHistoryResponse
                {
                    SessionId = session.Id,
                    Role = session.Role,
                    Difficulty = session.Difficulty,
                    StartedAt = session.StartedAt,
                    CompletedAt = session.CompletedAt,
                    Status = session.Status,
                    CompletionPercentage = result?.CompletionPercentage
                });
            }

            return historyList;
        }

        public async Task<InterviewDetailResponse?> GetInterviewDetailsAsync(Guid sessionId, Guid userId)
        {
            var session = await _interviewRepository.GetByIdAsync(sessionId);

            if (session == null || session.UserId != userId)
            {
                return null;
            }

            var questions = await _questionRepository.GetBySessionIdAsync(sessionId);
            var answers = await _answerRepository.GetBySessionIdAsync(sessionId);
            var result = await _resultRepository.GetBySessionIdAsync(sessionId);

            var questionsWithAnswers = questions.Select(q =>
            {
                var answer = answers.FirstOrDefault(a => a.InterviewQuestionId == q.Id);
                return new QuestionWithAnswerResponse
                {
                    Id = q.Id,
                    OrderNumber = q.OrderNumber,
                    Category = q.Category,
                    QuestionText = q.QuestionText,
                    IsAnswered = q.IsAnswered,
                    Answer = answer != null ? new AnswerResponse
                    {
                        Id = answer.Id,
                        QuestionId = answer.InterviewQuestionId,
                        Transcript = answer.Transcript,
                        AnsweredAt = answer.AnsweredAt
                    } : null
                };
            }).ToList();

            return new InterviewDetailResponse
            {
                SessionId = session.Id,
                Role = session.Role,
                Topic = session.Topic,
                Difficulty = session.Difficulty,
                QuestionCount = session.QuestionCount,
                DurationMinutes = session.DurationMinutes,
                Status = session.Status,
                StartedAt = session.StartedAt,
                CompletedAt = session.CompletedAt,
                Questions = questionsWithAnswers,
                Result = result != null ? new InterviewResultResponse
                {
                    TotalQuestions = result.TotalQuestions,
                    AnsweredQuestions = result.AnsweredQuestions,
                    CompletionPercentage = result.CompletionPercentage,
                    GeneratedAt = result.GeneratedAt
                } : null
            };
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
