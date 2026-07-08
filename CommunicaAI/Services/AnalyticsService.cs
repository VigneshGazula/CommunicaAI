using CommunicaAI.DTO.Analytics;
using CommunicaAI.Repositories.Interfaces;
using CommunicaAI.Services.Interfaces;

namespace CommunicaAI.Services;

public class AnalyticsService : IAnalyticsService
{
    private readonly IInterviewRepository _interviewRepository;
    private readonly IInterviewResultRepository _resultRepository;
    private readonly IAnswerEvaluationRepository _evaluationRepository;

    public AnalyticsService(
        IInterviewRepository interviewRepository,
        IInterviewResultRepository resultRepository,
        IAnswerEvaluationRepository evaluationRepository)
    {
        _interviewRepository = interviewRepository;
        _resultRepository = resultRepository;
        _evaluationRepository = evaluationRepository;
    }

    public async Task<PerformanceAnalyticsResponse> GetUserPerformanceAnalyticsAsync(Guid userId)
    {
        // Get all user sessions
        var sessions = await _interviewRepository.GetByUserIdAsync(userId);
        var completedSessions = sessions
            .Where(s => s.Status.ToLower() == "completed" && s.CompletedAt.HasValue)
            .OrderBy(s => s.CompletedAt)
            .ToList();

        if (!completedSessions.Any())
        {
            return new PerformanceAnalyticsResponse();
        }

        // Get all results for completed sessions
        var sessionIds = completedSessions.Select(s => s.Id).ToList();
        var allResults = new List<Models.InterviewResult>();

        foreach (var sessionId in sessionIds)
        {
            var result = await _resultRepository.GetBySessionIdAsync(sessionId);
            if (result != null)
            {
                allResults.Add(result);
            }
        }

        if (!allResults.Any())
        {
            return new PerformanceAnalyticsResponse();
        }

        var response = new PerformanceAnalyticsResponse
        {
            OverallProgress = await CalculateOverallProgressAsync(completedSessions, allResults),
            TechnicalScoreTrend = CalculateTrend(completedSessions, allResults, r => r.TechnicalScore),
            CommunicationScoreTrend = CalculateTrend(completedSessions, allResults, r => r.CommunicationScore),
            ConfidenceScoreTrend = CalculateTrend(completedSessions, allResults, r => r.ConfidenceScore),
            VideoAnalysisTrend = CalculateVideoTrend(completedSessions, allResults),
            ResumeMatchTrend = CalculateTrend(completedSessions, allResults, r => r.ResumeMatchScore, true),
            CompanyReadinessTrend = CalculateTrend(completedSessions, allResults, r => r.CompanyReadinessScore, true),
            StrongestSkills = await CalculateStrongestSkillsAsync(sessionIds),
            WeakestSkills = await CalculateWeakestSkillsAsync(sessionIds),
            PracticeRecommendations = CalculatePracticeRecommendations(completedSessions, allResults),
            WeeklyProgress = CalculateWeeklyProgress(completedSessions, allResults)
        };

        return response;
    }

    private async Task<OverallProgressData> CalculateOverallProgressAsync(
        List<Models.InterviewSession> sessions,
        List<Models.InterviewResult> results)
    {
        var totalInterviews = sessions.Count;
        var completedInterviews = sessions.Count(s => s.Status.ToLower() == "completed");

        var avgOverall = results.Any() ? (int)results.Average(r => r.OverallScore) : 0;
        var avgTechnical = results.Any() ? (int)results.Average(r => r.TechnicalScore) : 0;
        var avgCommunication = results.Any() ? (int)results.Average(r => r.CommunicationScore) : 0;
        var avgConfidence = results.Any() ? (int)results.Average(r => r.ConfidenceScore) : 0;

        var streak = CalculateCurrentStreak(sessions);
        var longestStreak = CalculateLongestStreak(sessions);
        var improvementRate = CalculateImprovementRate(results);

        return new OverallProgressData
        {
            TotalInterviews = totalInterviews,
            CompletedInterviews = completedInterviews,
            AverageOverallScore = avgOverall,
            AverageTechnicalScore = avgTechnical,
            AverageCommunicationScore = avgCommunication,
            AverageConfidenceScore = avgConfidence,
            CurrentStreak = streak,
            LongestStreak = longestStreak,
            ImprovementRate = improvementRate
        };
    }

    private List<TrendDataPoint> CalculateTrend(
        List<Models.InterviewSession> sessions,
        List<Models.InterviewResult> results,
        Func<Models.InterviewResult, int> scoreSelector,
        bool filterZeros = false)
    {
        var trend = new List<TrendDataPoint>();

        foreach (var session in sessions)
        {
            var result = results.FirstOrDefault(r => r.InterviewSessionId == session.Id);
            if (result == null) continue;

            var score = scoreSelector(result);
            if (filterZeros && score == 0) continue;

            trend.Add(new TrendDataPoint
            {
                Date = session.CompletedAt!.Value,
                Score = score,
                Role = session.Role,
                Difficulty = session.Difficulty
            });
        }

        return trend.OrderBy(t => t.Date).ToList();
    }

    private List<TrendDataPoint> CalculateVideoTrend(
        List<Models.InterviewSession> sessions,
        List<Models.InterviewResult> results)
    {
        var trend = new List<TrendDataPoint>();

        foreach (var session in sessions)
        {
            var result = results.FirstOrDefault(r => r.InterviewSessionId == session.Id);
            if (result == null) continue;

            // Calculate average video score from all video metrics
            var videoScores = new List<int>();
            if (result.EyeContactScore > 0) videoScores.Add(result.EyeContactScore);
            if (result.PostureScore > 0) videoScores.Add(result.PostureScore);
            if (result.FacialExpressionScore > 0) videoScores.Add(result.FacialExpressionScore);
            if (result.VideoConfidenceScore > 0) videoScores.Add(result.VideoConfidenceScore);

            if (!videoScores.Any()) continue;

            trend.Add(new TrendDataPoint
            {
                Date = session.CompletedAt!.Value,
                Score = (int)videoScores.Average(),
                Role = session.Role,
                Difficulty = session.Difficulty
            });
        }

        return trend.OrderBy(t => t.Date).ToList();
    }

    private async Task<List<SkillData>> CalculateStrongestSkillsAsync(List<Guid> sessionIds)
    {
        var skills = new Dictionary<string, (int totalScore, int count)>();

        foreach (var sessionId in sessionIds)
        {
            var evaluations = await _evaluationRepository.GetBySessionIdAsync(sessionId);

            foreach (var eval in evaluations)
            {
                AddOrUpdateSkill(skills, "Technical Knowledge", eval.TechnicalScore);
                AddOrUpdateSkill(skills, "Communication", eval.CommunicationScore);
                AddOrUpdateSkill(skills, "Confidence", eval.ConfidenceScore);
                AddOrUpdateSkill(skills, "Clarity", eval.ClarityScore);
                AddOrUpdateSkill(skills, "Completeness", eval.CompletenessScore);
                AddOrUpdateSkill(skills, "Grammar", eval.GrammarScore);
                AddOrUpdateSkill(skills, "Vocabulary", eval.VocabularyScore);
                AddOrUpdateSkill(skills, "Professionalism", eval.ProfessionalismScore);
                AddOrUpdateSkill(skills, "Answer Structure", eval.AnswerStructureScore);
                AddOrUpdateSkill(skills, "Persuasiveness", eval.PersuasivenessScore);
                AddOrUpdateSkill(skills, "Conciseness", eval.ConcisenessScore);
            }
        }

        return skills
            .Select(s => new SkillData
            {
                SkillName = s.Key,
                AverageScore = s.Value.totalScore / s.Value.count,
                Frequency = s.Value.count,
                Category = GetSkillCategory(s.Key)
            })
            .OrderByDescending(s => s.AverageScore)
            .Take(5)
            .ToList();
    }

    private async Task<List<SkillData>> CalculateWeakestSkillsAsync(List<Guid> sessionIds)
    {
        var skills = new Dictionary<string, (int totalScore, int count)>();

        foreach (var sessionId in sessionIds)
        {
            var evaluations = await _evaluationRepository.GetBySessionIdAsync(sessionId);

            foreach (var eval in evaluations)
            {
                AddOrUpdateSkill(skills, "Technical Knowledge", eval.TechnicalScore);
                AddOrUpdateSkill(skills, "Communication", eval.CommunicationScore);
                AddOrUpdateSkill(skills, "Confidence", eval.ConfidenceScore);
                AddOrUpdateSkill(skills, "Clarity", eval.ClarityScore);
                AddOrUpdateSkill(skills, "Completeness", eval.CompletenessScore);
                AddOrUpdateSkill(skills, "Grammar", eval.GrammarScore);
                AddOrUpdateSkill(skills, "Vocabulary", eval.VocabularyScore);
                AddOrUpdateSkill(skills, "Professionalism", eval.ProfessionalismScore);
                AddOrUpdateSkill(skills, "Answer Structure", eval.AnswerStructureScore);
                AddOrUpdateSkill(skills, "Persuasiveness", eval.PersuasivenessScore);
                AddOrUpdateSkill(skills, "Conciseness", eval.ConcisenessScore);
            }
        }

        return skills
            .Select(s => new SkillData
            {
                SkillName = s.Key,
                AverageScore = s.Value.totalScore / s.Value.count,
                Frequency = s.Value.count,
                Category = GetSkillCategory(s.Key)
            })
            .OrderBy(s => s.AverageScore)
            .Take(5)
            .ToList();
    }

    private PracticeRecommendationsData CalculatePracticeRecommendations(
        List<Models.InterviewSession> sessions,
        List<Models.InterviewResult> results)
    {
        var focusAreas = new List<string>();
        var topicsToImprove = new List<string>();

        if (!results.Any())
        {
            return new PracticeRecommendationsData
            {
                NextStepsSummary = "Start your first interview to get personalized recommendations."
            };
        }

        // Analyze weakest areas
        var avgTechnical = results.Average(r => r.TechnicalScore);
        var avgCommunication = results.Average(r => r.CommunicationScore);
        var avgConfidence = results.Average(r => r.ConfidenceScore);

        if (avgTechnical < 70) focusAreas.Add("Technical Knowledge");
        if (avgCommunication < 70) focusAreas.Add("Communication Skills");
        if (avgConfidence < 70) focusAreas.Add("Confidence Building");

        // Check video scores
        var videoResults = results.Where(r => r.EyeContactScore > 0 || r.PostureScore > 0).ToList();
        if (videoResults.Any())
        {
            var avgEyeContact = videoResults.Average(r => r.EyeContactScore);
            var avgPosture = videoResults.Average(r => r.PostureScore);

            if (avgEyeContact < 70) topicsToImprove.Add("Eye Contact");
            if (avgPosture < 70) topicsToImprove.Add("Body Language");
        }

        // Recommend next difficulty
        var recentResults = results.OrderByDescending(r => r.GeneratedAt).Take(3).ToList();
        var recentAvg = recentResults.Average(r => r.OverallScore);
        var mostCommonRole = sessions.GroupBy(s => s.Role).OrderByDescending(g => g.Count()).First().Key;

        var recommendedDifficulty = recentAvg >= 80 ? "Hard" : recentAvg >= 60 ? "Medium" : "Easy";

        var nextSteps = focusAreas.Any()
            ? $"Focus on improving {string.Join(", ", focusAreas)}. Practice {recommendedDifficulty.ToLower()} level interviews for {mostCommonRole}."
            : $"Great progress! Challenge yourself with {recommendedDifficulty.ToLower()} level interviews to continue growing.";

        return new PracticeRecommendationsData
        {
            FocusAreas = focusAreas,
            RecommendedRole = mostCommonRole,
            RecommendedDifficulty = recommendedDifficulty,
            TopicsToImprove = topicsToImprove,
            NextStepsSummary = nextSteps
        };
    }

    private WeeklyProgressData CalculateWeeklyProgress(
        List<Models.InterviewSession> sessions,
        List<Models.InterviewResult> results)
    {
        var today = DateTime.UtcNow.Date;
        var weekStart = today.AddDays(-(int)today.DayOfWeek);
        var lastWeekStart = weekStart.AddDays(-7);

        var thisWeekSessions = sessions.Where(s => s.CompletedAt >= weekStart).ToList();
        var lastWeekSessions = sessions.Where(s => s.CompletedAt >= lastWeekStart && s.CompletedAt < weekStart).ToList();

        var thisWeekResults = results.Where(r => 
            thisWeekSessions.Any(s => s.Id == r.InterviewSessionId)).ToList();
        var lastWeekResults = results.Where(r => 
            lastWeekSessions.Any(s => s.Id == r.InterviewSessionId)).ToList();

        var avgThisWeek = thisWeekResults.Any() ? (int)thisWeekResults.Average(r => r.OverallScore) : 0;
        var avgLastWeek = lastWeekResults.Any() ? (int)lastWeekResults.Average(r => r.OverallScore) : 0;

        var improvement = avgLastWeek > 0 
            ? (int)Math.Round(((double)(avgThisWeek - avgLastWeek) / avgLastWeek) * 100) 
            : 0;

        return new WeeklyProgressData
        {
            InterviewsThisWeek = thisWeekSessions.Count,
            InterviewsLastWeek = lastWeekSessions.Count,
            AverageScoreThisWeek = avgThisWeek,
            AverageScoreLastWeek = avgLastWeek,
            WeekOverWeekImprovement = improvement
        };
    }

    private int CalculateCurrentStreak(List<Models.InterviewSession> sessions)
    {
        var completed = sessions
            .Where(s => s.Status.ToLower() == "completed" && s.CompletedAt.HasValue)
            .OrderByDescending(s => s.CompletedAt)
            .ToList();

        if (!completed.Any()) return 0;

        var streak = 0;
        var today = DateTime.UtcNow.Date;

        foreach (var session in completed)
        {
            var sessionDate = session.CompletedAt!.Value.Date;
            var daysDiff = (today - sessionDate).Days;

            if (daysDiff <= streak + 1)
            {
                streak++;
            }
            else
            {
                break;
            }
        }

        return streak;
    }

    private int CalculateLongestStreak(List<Models.InterviewSession> sessions)
    {
        var completed = sessions
            .Where(s => s.Status.ToLower() == "completed" && s.CompletedAt.HasValue)
            .OrderBy(s => s.CompletedAt)
            .Select(s => s.CompletedAt!.Value.Date)
            .Distinct()
            .ToList();

        if (!completed.Any()) return 0;

        var longestStreak = 1;
        var currentStreak = 1;

        for (int i = 1; i < completed.Count; i++)
        {
            var daysDiff = (completed[i] - completed[i - 1]).Days;

            if (daysDiff == 1)
            {
                currentStreak++;
                longestStreak = Math.Max(longestStreak, currentStreak);
            }
            else
            {
                currentStreak = 1;
            }
        }

        return longestStreak;
    }

    private double CalculateImprovementRate(List<Models.InterviewResult> results)
    {
        if (results.Count < 2) return 0;

        var orderedResults = results.OrderBy(r => r.GeneratedAt).ToList();
        var recentCount = Math.Min(5, orderedResults.Count);
        var recent = orderedResults.TakeLast(recentCount).ToList();
        var previous = orderedResults.SkipLast(recentCount).TakeLast(recentCount).ToList();

        if (!previous.Any()) return 0;

        var recentAvg = recent.Average(r => r.OverallScore);
        var previousAvg = previous.Average(r => r.OverallScore);

        return previousAvg > 0 
            ? Math.Round(((recentAvg - previousAvg) / previousAvg) * 100, 2) 
            : 0;
    }

    private void AddOrUpdateSkill(Dictionary<string, (int totalScore, int count)> skills, string skillName, int score)
    {
        if (skills.ContainsKey(skillName))
        {
            var current = skills[skillName];
            skills[skillName] = (current.totalScore + score, current.count + 1);
        }
        else
        {
            skills[skillName] = (score, 1);
        }
    }

    private string GetSkillCategory(string skillName)
    {
        return skillName switch
        {
            "Technical Knowledge" or "Clarity" or "Completeness" => "Technical",
            "Communication" or "Grammar" or "Vocabulary" or "Professionalism" or "Answer Structure" or "Persuasiveness" or "Conciseness" => "Communication",
            "Confidence" => "Confidence",
            _ => "General"
        };
    }
}
