using CommunicaAI.Services;
using CommunicaAI.Models;

namespace CommunicaAI.Services.Interfaces
{
    public interface IGeminiService
    {
        Task<SubmitAudioAnswerResponse> EvaluateAnswerAsync(
        string question,
        string answer);

        Task<SubmitAudioAnswerResponse> EvaluateAnswerAsync(
        string question,
        string answer,
        string interviewType);

        Task<CoachingReport> GenerateCoachingReportAsync(
            string role,
            string difficulty,
            List<QuestionAnswerPair> qaList,
            Dictionary<string, int> aggregateScores);

        Task<CompanyEvaluationResult> EvaluateCompanyFitAsync(
            CompanyProfile companyProfile,
            string role,
            string difficulty,
            List<QuestionAnswerPair> qaList,
            Dictionary<string, int> aggregateScores);

        Task<ResumeAnalysisResult> AnalyzeResumeMatchAsync(
            ResumeProfile resumeProfile,
            string role,
            string difficulty,
            List<QuestionAnswerPair> qaList,
            Dictionary<string, int> aggregateScores);
    }
}
