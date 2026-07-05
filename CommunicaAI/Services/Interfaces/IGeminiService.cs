using CommunicaAI.Services;

namespace CommunicaAI.Services.Interfaces
{
    public interface IGeminiService
    {
        Task<SubmitAudioAnswerResponse> EvaluateAnswerAsync(
        string question,
        string answer);

        Task<CoachingReport> GenerateCoachingReportAsync(
            string role,
            string difficulty,
            List<QuestionAnswerPair> qaList,
            Dictionary<string, int> aggregateScores);
    }
}
