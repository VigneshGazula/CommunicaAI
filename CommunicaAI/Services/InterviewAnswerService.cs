using CommunicaAI.DTO.Interview;
using CommunicaAI.Models;
using CommunicaAI.Repositories.Interfaces;
using CommunicaAI.Services.Interfaces;

namespace CommunicaAI.Services
{
    public class InterviewAnswerService : IInterviewAnswerService
    {
        private readonly IInterviewAnswerRepository _answerRepository;
        private readonly IInterviewQuestionRepository _questionRepository;
        private readonly IInterviewRepository _interviewRepository;

        public InterviewAnswerService(
            IInterviewAnswerRepository answerRepository,
            IInterviewQuestionRepository questionRepository,
            IInterviewRepository interviewRepository)
        {
            _answerRepository = answerRepository;
            _questionRepository = questionRepository;
            _interviewRepository = interviewRepository;
        }

        public async Task<AnswerResponse> SubmitAnswerAsync(Guid sessionId, Guid userId, AnswerSubmitRequest request)
        {
            var session = await _interviewRepository.GetByIdAsync(sessionId);
            if (session == null || session.UserId != userId)
            {
                throw new UnauthorizedAccessException("Session not found or unauthorized");
            }

            var question = await _questionRepository.GetBySessionAndQuestionIdAsync(sessionId, request.QuestionId);
            if (question == null)
            {
                throw new InvalidOperationException("Question does not belong to this session");
            }

            var existingAnswer = await _answerRepository.GetByQuestionIdAsync(request.QuestionId);
            if (existingAnswer != null)
            {
                throw new InvalidOperationException("Question already answered");
            }

            var answer = new InterviewAnswer
            {
                Id = Guid.NewGuid(),
                InterviewQuestionId = request.QuestionId,
                InterviewSessionId = sessionId,
                Transcript = request.Transcript,
                AnsweredAt = DateTime.UtcNow
            };

            var created = await _answerRepository.CreateAsync(answer);

            question.IsAnswered = true;
            await _questionRepository.UpdateAsync(question);

            return new AnswerResponse
            {
                Id = created.Id,
                QuestionId = created.InterviewQuestionId,
                Transcript = created.Transcript,
                AnsweredAt = created.AnsweredAt
            };
        }
    }
}
