using CommunicaAI.DTO.Interview;
using CommunicaAI.Models;
using CommunicaAI.Repositories.Interfaces;
using CommunicaAI.Services.Interfaces;

namespace CommunicaAI.Services
{
    public class InterviewQuestionService : IInterviewQuestionService
    {
        private readonly IInterviewQuestionRepository _questionRepository;
        private readonly IInterviewRepository _interviewRepository;
        private readonly IQuestionBankRepository _questionBankRepository;
        private static readonly Random _random = new Random();

        public InterviewQuestionService(
            IInterviewQuestionRepository questionRepository,
            IInterviewRepository interviewRepository,
            IQuestionBankRepository questionBankRepository)
        {
            _questionRepository = questionRepository;
            _interviewRepository = interviewRepository;
            _questionBankRepository = questionBankRepository;
        }

        public async Task<List<QuestionResponse>> GetSessionQuestionsAsync(Guid sessionId, Guid userId)
        {
            var session = await _interviewRepository.GetByIdAsync(sessionId);
            if (session == null || session.UserId != userId)
            {
                return new List<QuestionResponse>();
            }

            var questions = await _questionRepository.GetBySessionIdAsync(sessionId);
            return questions.Select(MapToResponse).ToList();
        }

        public async Task GenerateQuestionsForSessionAsync(Guid sessionId, string role, string difficulty, int questionCount)
        {
            Console.WriteLine("========== QUESTION GENERATION ==========");
            Console.WriteLine($"SessionId   : {sessionId}");
            Console.WriteLine($"Role        : {role}");
            Console.WriteLine($"Difficulty  : {difficulty}");
            Console.WriteLine($"QuestionCnt : {questionCount}");

            var technicalCount = (int)Math.Ceiling(questionCount * 0.6);
            var behavioralCount = (int)Math.Ceiling(questionCount * 0.2);
            var hrCount = questionCount - technicalCount - behavioralCount;

            var questions = new List<InterviewQuestion>();
            int orderNumber = 1;

            var technicalQuestions = await GetRandomQuestionsAsync(role, difficulty, "Technical", technicalCount);
            foreach (var question in CreateInterviewQuestions(sessionId, technicalQuestions, orderNumber))
            {
                questions.Add(question);
                orderNumber++;
            }
            Console.WriteLine($"Technical Found : {technicalQuestions.Count}");

            var behavioralQuestions = await GetRandomQuestionsAsync(role, difficulty, "Behavioral", behavioralCount);
            foreach (var question in CreateInterviewQuestions(sessionId, behavioralQuestions, orderNumber))
            {
                questions.Add(question);
                orderNumber++;
            }
            Console.WriteLine($"Behavioral Found : {behavioralQuestions.Count}");

            var hrQuestions = await GetRandomQuestionsAsync(role, difficulty, "HR", hrCount);
            foreach (var question in CreateInterviewQuestions(sessionId, hrQuestions, orderNumber))
            {
                questions.Add(question);
                orderNumber++;
            }
            Console.WriteLine($"HR Found : {hrQuestions.Count}");

            if (questions.Count < questionCount)
            {
                var remainingCount = questionCount - questions.Count;
                var fallbackQuestions = await GetRandomQuestionsAsync(role, difficulty, null, remainingCount);
                foreach (var question in CreateInterviewQuestions(sessionId, fallbackQuestions, orderNumber))
                {
                    questions.Add(question);
                    orderNumber++;
                }
            }
            Console.WriteLine($"InterviewQuestions Created : {questions.Count}");

            await _questionRepository.CreateRangeAsync(questions);

            Console.WriteLine("Questions saved successfully.");
        }

        private async Task<List<QuestionBank>> GetRandomQuestionsAsync(string role, string difficulty, string? category, int count)
        {
            List<QuestionBank> availableQuestions;

            if (category != null)
            {
                availableQuestions = await _questionBankRepository.GetByRoleDifficultyAndCategoryAsync(role, difficulty, category);
            }
            else
            {
                availableQuestions = await _questionBankRepository.GetByRoleAndDifficultyAsync(role, difficulty);
            }

            if (availableQuestions.Count == 0)
            {
                return new List<QuestionBank>();
            }

            // Use Guid-based shuffling for better randomization
            return availableQuestions
                .OrderBy(_ => Guid.NewGuid())
                .Take(count)
                .ToList();
        }

        private static List<InterviewQuestion> CreateInterviewQuestions(Guid sessionId, List<QuestionBank> bankQuestions, int startOrder)
        {
            int order = startOrder;
            return bankQuestions.Select(bq => new InterviewQuestion
            {
                Id = Guid.NewGuid(),
                InterviewSessionId = sessionId,
                OrderNumber = order++,
                Category = bq.Category,
                QuestionText = bq.QuestionText,
                IsAnswered = false,
                CreatedAt = DateTime.UtcNow
            }).ToList();
        }

        private static QuestionResponse MapToResponse(InterviewQuestion question)
        {
            return new QuestionResponse
            {
                Id = question.Id,
                OrderNumber = question.OrderNumber,
                Category = question.Category,
                QuestionText = question.QuestionText,
                IsAnswered = question.IsAnswered
            };
        }
    }
}
