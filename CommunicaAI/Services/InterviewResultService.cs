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
        private readonly IInterviewRepository _interviewRepository;
        private readonly ICompanyProfileRepository _companyRepository;

        public InterviewResultService(
            IInterviewResultRepository resultRepository,
            IInterviewQuestionRepository questionRepository,
            IInterviewAnswerRepository answerRepository,
            IAnswerEvaluationRepository evaluationRepository,
            IGeminiService geminiService,
            IInterviewRepository interviewRepository,
            ICompanyProfileRepository companyRepository)
        {
            _resultRepository = resultRepository;
            _questionRepository = questionRepository;
            _answerRepository = answerRepository;
            _evaluationRepository = evaluationRepository;
            _geminiService = geminiService;
            _interviewRepository = interviewRepository;
            _companyRepository = companyRepository;
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

            // Generate AI Coaching Report (Module 5)
            var interviewSession = await _interviewRepository.GetByIdAsync(sessionId);
            if (interviewSession != null)
            {
                await GenerateCoachingReportAsync(created.Id, interviewSession, evaluations);
                
                // Generate Company Intelligence Report (Module 6)
                if (interviewSession.CompanyProfileId.HasValue)
                {
                    await GenerateCompanyEvaluationAsync(created.Id, interviewSession, evaluations);
                }
            }

            return MapToResponse(created);
        }

        private async Task GenerateCoachingReportAsync(
            Guid resultId,
            InterviewSession session,
            List<AnswerEvaluation> evaluations)
        {
            try
            {
                // Prepare question-answer pairs
                var qaList = new List<QuestionAnswerPair>();
                var answers = await _answerRepository.GetBySessionIdAsync(session.Id);
                var questions = await _questionRepository.GetBySessionIdAsync(session.Id);

                foreach (var answer in answers)
                {
                    var question = questions.FirstOrDefault(q => q.Id == answer.InterviewQuestionId);
                    var evaluation = evaluations.FirstOrDefault(e => e.InterviewAnswerId == answer.Id);

                    if (question != null && evaluation != null && !string.IsNullOrWhiteSpace(answer.Transcript))
                    {
                        qaList.Add(new QuestionAnswerPair
                        {
                            Question = question.QuestionText,
                            Answer = answer.Transcript,
                            TechnicalScore = evaluation.TechnicalScore,
                            CommunicationScore = evaluation.CommunicationScore,
                            GrammarScore = evaluation.GrammarScore,
                            ConfidenceScore = evaluation.ConfidenceScore
                        });
                    }
                }

                if (qaList.Count == 0)
                {
                    return; // No data to coach on
                }

                // Prepare aggregate scores
                var aggregateScores = new Dictionary<string, int>();
                if (evaluations.Any())
                {
                    aggregateScores["Technical"] = (int)evaluations.Average(e => e.TechnicalScore);
                    aggregateScores["Communication"] = (int)evaluations.Average(e => e.CommunicationScore);
                    aggregateScores["Confidence"] = (int)evaluations.Average(e => e.ConfidenceScore);
                    aggregateScores["Grammar"] = (int)evaluations.Average(e => e.GrammarScore);
                    aggregateScores["Vocabulary"] = (int)evaluations.Average(e => e.VocabularyScore);
                    aggregateScores["Professionalism"] = (int)evaluations.Average(e => e.ProfessionalismScore);
                }

                // Generate coaching report
                var coachingReport = await _geminiService.GenerateCoachingReportAsync(
                    session.Role,
                    session.Difficulty,
                    qaList,
                    aggregateScores
                );

                // Update result with coaching data
                var result = await _resultRepository.GetByIdAsync(resultId);
                if (result != null)
                {
                    result.CoachingSummary = coachingReport.OverallSummary;
                    result.CoachingStrengths = coachingReport.TopStrengths;
                    result.CoachingWeaknesses = coachingReport.KeyWeaknesses;
                    result.CommunicationImprovements = coachingReport.CommunicationImprovements;
                    result.TechnicalImprovements = coachingReport.TechnicalImprovements;
                    result.VideoImprovements = coachingReport.VideoImprovements;
                    result.VoiceImprovements = coachingReport.VoiceImprovements;
                    result.PracticeRecommendations = coachingReport.PracticeRecommendations;
                    result.SuggestedRole = coachingReport.SuggestedRole;
                    result.SuggestedDifficulty = coachingReport.SuggestedDifficulty;
                    result.SuggestedQuestionCount = coachingReport.SuggestedQuestionCount;
                    result.LearningResources = coachingReport.LearningResources;
                    result.MotivationalMessage = coachingReport.MotivationalMessage;

                    await _resultRepository.UpdateAsync(result);
                }
            }
            catch (Exception ex)
            {
                // Log error but don't fail the result generation
                Console.WriteLine($"Failed to generate coaching report: {ex.Message}");
            }
        }

        private async Task GenerateCompanyEvaluationAsync(
            Guid resultId,
            InterviewSession session,
            List<AnswerEvaluation> evaluations)
        {
            try
            {
                var companyProfile = await _companyRepository.GetByIdAsync(session.CompanyProfileId!.Value);
                if (companyProfile == null)
                {
                    return;
                }

                // Prepare question-answer pairs
                var qaList = new List<QuestionAnswerPair>();
                var answers = await _answerRepository.GetBySessionIdAsync(session.Id);
                var questions = await _questionRepository.GetBySessionIdAsync(session.Id);

                foreach (var answer in answers)
                {
                    var question = questions.FirstOrDefault(q => q.Id == answer.InterviewQuestionId);
                    var evaluation = evaluations.FirstOrDefault(e => e.InterviewAnswerId == answer.Id);

                    if (question != null && evaluation != null && !string.IsNullOrWhiteSpace(answer.Transcript))
                    {
                        qaList.Add(new QuestionAnswerPair
                        {
                            Question = question.QuestionText,
                            Answer = answer.Transcript,
                            TechnicalScore = evaluation.TechnicalScore,
                            CommunicationScore = evaluation.CommunicationScore,
                            GrammarScore = evaluation.GrammarScore,
                            ConfidenceScore = evaluation.ConfidenceScore
                        });
                    }
                }

                if (qaList.Count == 0)
                {
                    return;
                }

                // Prepare aggregate scores
                var aggregateScores = new Dictionary<string, int>();
                if (evaluations.Any())
                {
                    aggregateScores["Technical"] = (int)evaluations.Average(e => e.TechnicalScore);
                    aggregateScores["Communication"] = (int)evaluations.Average(e => e.CommunicationScore);
                    aggregateScores["Confidence"] = (int)evaluations.Average(e => e.ConfidenceScore);
                    aggregateScores["Grammar"] = (int)evaluations.Average(e => e.GrammarScore);
                    aggregateScores["Vocabulary"] = (int)evaluations.Average(e => e.VocabularyScore);
                    aggregateScores["Professionalism"] = (int)evaluations.Average(e => e.ProfessionalismScore);
                }

                // Generate company evaluation
                var companyEvaluation = await _geminiService.EvaluateCompanyFitAsync(
                    companyProfile,
                    session.Role,
                    session.Difficulty,
                    qaList,
                    aggregateScores
                );

                // Update result with company evaluation data
                var result = await _resultRepository.GetByIdAsync(resultId);
                if (result != null)
                {
                    result.CompanyReadinessScore = companyEvaluation.CompanyReadinessScore;
                    result.TechnicalAlignment = companyEvaluation.TechnicalAlignment;
                    result.CommunicationAlignment = companyEvaluation.CommunicationAlignment;
                    result.CultureFit = companyEvaluation.CultureFit;
                    result.CompanySpecificFeedback = companyEvaluation.CompanySpecificFeedback;

                    await _resultRepository.UpdateAsync(result);
                }
            }
            catch (Exception ex)
            {
                // Log error but don't fail the result generation
                Console.WriteLine($"Failed to generate company evaluation: {ex.Message}");
            }
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
                EyeContactScore = result.EyeContactScore,
                PostureScore = result.PostureScore,
                FacialExpressionScore = result.FacialExpressionScore,
                VideoConfidenceScore = result.VideoConfidenceScore,
                VideoFeedback = result.VideoFeedback,
                Strengths = result.Strengths,
                Weaknesses = result.Weaknesses,
                Recommendations = result.Recommendations,
                Summary = result.Summary,
                // AI Coach fields
                CoachingSummary = result.CoachingSummary,
                CoachingStrengths = result.CoachingStrengths,
                CoachingWeaknesses = result.CoachingWeaknesses,
                CommunicationImprovements = result.CommunicationImprovements,
                TechnicalImprovements = result.TechnicalImprovements,
                VideoImprovements = result.VideoImprovements,
                VoiceImprovements = result.VoiceImprovements,
                PracticeRecommendations = result.PracticeRecommendations,
                SuggestedRole = result.SuggestedRole,
                SuggestedDifficulty = result.SuggestedDifficulty,
                SuggestedQuestionCount = result.SuggestedQuestionCount,
                LearningResources = result.LearningResources,
                MotivationalMessage = result.MotivationalMessage,
                // Company Intelligence (Module 6)
                CompanyReadinessScore = result.CompanyReadinessScore,
                TechnicalAlignment = result.TechnicalAlignment,
                CommunicationAlignment = result.CommunicationAlignment,
                CultureFit = result.CultureFit,
                CompanySpecificFeedback = result.CompanySpecificFeedback
            };
        }
    }
}
