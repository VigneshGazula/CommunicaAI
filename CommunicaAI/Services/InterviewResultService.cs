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
        private readonly IInterviewAnswerRepository _answerRepository;
        private readonly IAnswerEvaluationRepository _evaluationRepository;
        private readonly IGeminiService _geminiService;

        public InterviewResultService(
            IInterviewResultRepository resultRepository,
            IInterviewQuestionRepository questionRepository,
            IInterviewAnswerRepository answerRepository,
            IAnswerEvaluationRepository evaluationRepository,
            IGeminiService geminiService)
        {
            _resultRepository = resultRepository;
            _questionRepository = questionRepository;
            _answerRepository = answerRepository;
            _evaluationRepository = evaluationRepository;
            _geminiService = geminiService;
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

            // Batch evaluate all answers
            var answers = await _answerRepository.GetBySessionIdAsync(sessionId);
            var evaluations = new List<AnswerEvaluation>();

            foreach (var answer in answers)
            {
                // Check if already evaluated
                var existing = await _evaluationRepository.GetByAnswerIdAsync(answer.Id);
                if (existing != null)
                {
                    evaluations.Add(existing);
                    continue;
                }

                // Find the question
                var question = questions.FirstOrDefault(q => q.Id == answer.InterviewQuestionId);
                if (question == null || string.IsNullOrWhiteSpace(answer.Transcript))
                {
                    continue;
                }

                // Evaluate answer
                var evaluation = await _geminiService.EvaluateAnswerAsync(
                    question.QuestionText,
                    answer.Transcript
                );

                var answerEvaluation = new AnswerEvaluation
                {
                    Id = Guid.NewGuid(),
                    InterviewAnswerId = answer.Id,
                    // Technical Evaluation
                    TechnicalScore = evaluation.TechnicalScore,
                    ClarityScore = evaluation.ClarityScore,
                    CompletenessScore = evaluation.CompletenessScore,
                    OverallScore = evaluation.OverallScore,
                    // AI Communication Evaluation
                    CommunicationScore = evaluation.CommunicationScore,
                    ConfidenceScore = evaluation.ConfidenceScore,
                    GrammarScore = evaluation.GrammarScore,
                    VocabularyScore = evaluation.VocabularyScore,
                    ProfessionalismScore = evaluation.ProfessionalismScore,
                    AnswerStructureScore = evaluation.AnswerStructureScore,
                    PersuasivenessScore = evaluation.PersuasivenessScore,
                    ConcisenessScore = evaluation.ConcisenessScore,
                    // Feedback
                    Strengths = evaluation.Strengths,
                    Improvements = evaluation.Improvements,
                    Feedback = evaluation.Feedback,
                    EvaluatedAt = DateTime.UtcNow
                };

                await _evaluationRepository.CreateAsync(answerEvaluation);
                evaluations.Add(answerEvaluation);
            }

            // Calculate aggregate scores
            int technicalScore = 0;
            int communicationScore = 0;
            int confidenceScore = 0;
            int overallScore = 0;
            
            if (evaluations.Any())
            {
                technicalScore = (int)evaluations.Average(e => e.TechnicalScore);
                communicationScore = (int)evaluations.Average(e => e.CommunicationScore);
                confidenceScore = (int)evaluations.Average(e => e.ConfidenceScore);
                overallScore = (int)evaluations.Average(e => e.OverallScore);
            }

            // Aggregate feedback
            var strengths = string.Join("; ", evaluations.Select(e => e.Strengths).Where(s => !string.IsNullOrWhiteSpace(s)));
            var weaknesses = string.Join("; ", evaluations.Select(e => e.Improvements).Where(s => !string.IsNullOrWhiteSpace(s)));
            var recommendations = GenerateRecommendations(overallScore);
            var summary = $"Completed {answeredQuestions} of {totalQuestions} questions with an overall score of {overallScore}%.";

            var result = new InterviewResult
            {
                Id = Guid.NewGuid(),
                InterviewSessionId = sessionId,
                TotalQuestions = totalQuestions,
                AnsweredQuestions = answeredQuestions,
                CompletionPercentage = completionPercentage,
                OverallScore = overallScore,
                TechnicalScore = technicalScore,
                CommunicationScore = communicationScore,
                ConfidenceScore = confidenceScore,
                Strengths = strengths,
                Weaknesses = weaknesses,
                Recommendations = recommendations,
                Summary = summary,
                GeneratedAt = DateTime.UtcNow
            };

            var created = await _resultRepository.CreateAsync(result);
            return MapToResponse(created);
        }

        private static string GenerateRecommendations(int overallScore)
        {
            if (overallScore >= 80)
            {
                return "Excellent performance! Continue practicing to maintain consistency.";
            }
            else if (overallScore >= 60)
            {
                return "Good performance. Focus on improving technical depth and clarity.";
            }
            else
            {
                return "Keep practicing. Focus on understanding core concepts and clear communication.";
            }
        }

        private static InterviewResultResponse MapToResponse(InterviewResult result)
        {
            return new InterviewResultResponse
            {
                TotalQuestions = result.TotalQuestions,
                AnsweredQuestions = result.AnsweredQuestions,
                CompletionPercentage = result.CompletionPercentage,
                GeneratedAt = result.GeneratedAt,
                OverallScore = result.OverallScore,
                TechnicalScore = result.TechnicalScore,
                CommunicationScore = result.CommunicationScore,
                ConfidenceScore = result.ConfidenceScore,
                Strengths = result.Strengths,
                Weaknesses = result.Weaknesses,
                Recommendations = result.Recommendations,
                Summary = result.Summary
            };
        }
    }
}
