using CommunicaAI.DTO.Interview;
using CommunicaAI.Models;
using CommunicaAI.Repositories.Interfaces;
using CommunicaAI.Services.Interfaces;

namespace CommunicaAI.Services
{
    public class InterviewResultService : IInterviewResultService
    {
        private readonly IInterviewResultRepository _resultRepository;
        private readonly IInterviewQuestionRepository _questionRepository;

        public InterviewResultService(
            IInterviewResultRepository resultRepository,
            IInterviewQuestionRepository questionRepository)
        {
            _resultRepository = resultRepository;
            _questionRepository = questionRepository;
        }

        public async Task<InterviewResultResponse> GenerateResultAsync(Guid sessionId)
        {
            var existingResult = await _resultRepository.GetBySessionIdAsync(sessionId);
            if (existingResult != null)
            {
                return MapToResponse(existingResult);
            }

            var questions = await _questionRepository.GetBySessionIdAsync(sessionId);
            var totalQuestions = questions.Count;
            var answeredQuestions = await _questionRepository.GetAnsweredCountAsync(sessionId);
            var completionPercentage = totalQuestions > 0 
                ? Math.Round((double)answeredQuestions / totalQuestions * 100, 2) 
                : 0;

            var result = new InterviewResult
            {
                Id = Guid.NewGuid(),
                InterviewSessionId = sessionId,
                TotalQuestions = totalQuestions,
                AnsweredQuestions = answeredQuestions,
                CompletionPercentage = completionPercentage,
                GeneratedAt = DateTime.UtcNow
            };

            var created = await _resultRepository.CreateAsync(result);
            return MapToResponse(created);
        }

        private static InterviewResultResponse MapToResponse(InterviewResult result)
        {
            return new InterviewResultResponse
            {
                TotalQuestions = result.TotalQuestions,
                AnsweredQuestions = result.AnsweredQuestions,
                CompletionPercentage = result.CompletionPercentage,
                GeneratedAt = result.GeneratedAt
            };
        }
    }
}
