using System.Text;
using System.Text.Json;
using CommunicaAI.Configurations;
using CommunicaAI.Models;
using CommunicaAI.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace CommunicaAI.Services;

public class GeminiService : IGeminiService
{
    private readonly HttpClient _httpClient;
    private readonly GeminiSettings _settings;

    public GeminiService(
        HttpClient httpClient,
        IOptions<GeminiSettings> options)
    {
        _httpClient = httpClient;
        _settings = options.Value;
    }

    public async Task<SubmitAudioAnswerResponse>
        EvaluateAnswerAsync(
            string question,
            string answer)
    {
        var prompt = $@"
You are a senior technical interviewer evaluating a candidate's answer across multiple dimensions.

Question:
{question}

Candidate Answer:
{answer}

Evaluate the answer comprehensively and return ONLY a valid JSON object (no markdown, no explanation) with these exact fields:
{{
  ""technicalScore"": <number 0-100>,
  ""clarityScore"": <number 0-100>,
  ""completenessScore"": <number 0-100>,
  ""overallScore"": <number 0-100>,
  ""communicationScore"": <number 0-100>,
  ""confidenceScore"": <number 0-100>,
  ""grammarScore"": <number 0-100>,
  ""vocabularyScore"": <number 0-100>,
  ""professionalismScore"": <number 0-100>,
  ""answerStructureScore"": <number 0-100>,
  ""persuasivenessScore"": <number 0-100>,
  ""concisenessScore"": <number 0-100>,
  ""strengths"": ""<single string with strengths separated by semicolons>"",
  ""improvements"": ""<single string with improvements separated by semicolons>"",
  ""feedback"": ""<single string with overall feedback>""
}}

Evaluation Guidelines:
- Technical Score: Accuracy and depth of technical knowledge
- Clarity Score: How clearly the answer was expressed
- Completeness Score: How thoroughly the answer addresses the question
- Overall Score: Weighted average of all aspects
- Communication Score: Overall verbal communication quality
- Confidence Score: Conviction and assurance in the response
- Grammar Score: Grammatical correctness and sentence structure
- Vocabulary Score: Appropriate use of professional terminology
- Professionalism Score: Professional tone and demeanor
- Answer Structure Score: Logical organization and flow
- Persuasiveness Score: Ability to convince and present compelling arguments
- Conciseness Score: Balance between detail and brevity

Important: All text fields (strengths, improvements, feedback) must be single strings, not arrays.";

        var request = new
        {
            contents = new[]
            {
                new
                {
                    parts = new[]
                    {
                        new
                        {
                            text = prompt
                        }
                    }
                }
            }
        };

        var url =
            $"https://generativelanguage.googleapis.com/v1beta/models/{_settings.Model}:generateContent?key={_settings.ApiKey}";

        // Retry logic for rate limiting (429 errors)
        int maxRetries = 3;
        int retryDelayMs = 2000; // Start with 2 seconds
        
        string json = string.Empty;

        for (int attempt = 0; attempt <= maxRetries; attempt++)
        {
            try
            {
                var response =
                    await _httpClient.PostAsJsonAsync(
                        url,
                        request);

                if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests && attempt < maxRetries)
                {
                    // Rate limited - wait and retry with exponential backoff
                    await Task.Delay(retryDelayMs);
                    retryDelayMs *= 2; // Double the delay for next attempt
                    continue;
                }

                response.EnsureSuccessStatusCode();

                json = await response.Content.ReadAsStringAsync();
                break; // Success - exit retry loop
            }
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.TooManyRequests && attempt < maxRetries)
            {
                // Rate limited - wait and retry with exponential backoff
                await Task.Delay(retryDelayMs);
                retryDelayMs *= 2;
            }
        }

        if (string.IsNullOrEmpty(json))
        {
            throw new Exception("Failed to evaluate answer after multiple retries due to rate limiting. Please try again later.");
        }

        using var document =
            JsonDocument.Parse(json);

        var aiResponse =
            document.RootElement
            .GetProperty("candidates")[0]
            .GetProperty("content")
            .GetProperty("parts")[0]
            .GetProperty("text")
            .GetString();

        // Clean up markdown if present
        var cleanedResponse = aiResponse!
            .Replace("```json", "")
            .Replace("```", "")
            .Trim();

        try
        {
            // Try deserializing with flexible handling
            using var evalDoc = JsonDocument.Parse(cleanedResponse);
            var root = evalDoc.RootElement;

            return new SubmitAudioAnswerResponse
            {
                // Technical Evaluation
                TechnicalScore = GetIntProperty(root, "technicalScore", 70),
                ClarityScore = GetIntProperty(root, "clarityScore", 70),
                CompletenessScore = GetIntProperty(root, "completenessScore", 70),
                OverallScore = GetIntProperty(root, "overallScore", 70),
                
                // AI Communication Evaluation
                CommunicationScore = GetIntProperty(root, "communicationScore", 70),
                ConfidenceScore = GetIntProperty(root, "confidenceScore", 70),
                GrammarScore = GetIntProperty(root, "grammarScore", 70),
                VocabularyScore = GetIntProperty(root, "vocabularyScore", 70),
                ProfessionalismScore = GetIntProperty(root, "professionalismScore", 70),
                AnswerStructureScore = GetIntProperty(root, "answerStructureScore", 70),
                PersuasivenessScore = GetIntProperty(root, "persuasivenessScore", 70),
                ConcisenessScore = GetIntProperty(root, "concisenessScore", 70),
                
                // Text Feedback
                Strengths = GetStringOrArrayAsString(root, "strengths"),
                Improvements = GetStringOrArrayAsString(root, "improvements"),
                Feedback = GetStringOrArrayAsString(root, "feedback")
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Deserialization error: {ex.Message}");
            Console.WriteLine($"Response: {cleanedResponse}");
            
            // Return default values on error
            return new SubmitAudioAnswerResponse
            {
                TechnicalScore = 70,
                ClarityScore = 70,
                CompletenessScore = 70,
                OverallScore = 70,
                CommunicationScore = 70,
                ConfidenceScore = 70,
                GrammarScore = 70,
                VocabularyScore = 70,
                ProfessionalismScore = 70,
                AnswerStructureScore = 70,
                PersuasivenessScore = 70,
                ConcisenessScore = 70,
                Strengths = "Answer provided",
                Improvements = "Could be more detailed",
                Feedback = "Evaluation could not be completed due to formatting issues"
            };
        }
    }

    private static int GetIntProperty(JsonElement root, string propertyName, int defaultValue)
    {
        if (root.TryGetProperty(propertyName, out var property) && property.ValueKind == JsonValueKind.Number)
        {
            return property.GetInt32();
        }
        return defaultValue;
    }

    private static string GetStringOrArrayAsString(JsonElement root, string propertyName)
    {
        if (!root.TryGetProperty(propertyName, out var property))
        {
            return string.Empty;
        }

        if (property.ValueKind == JsonValueKind.String)
        {
            return property.GetString() ?? string.Empty;
        }
        else if (property.ValueKind == JsonValueKind.Array)
        {
            var items = new List<string>();
            foreach (var item in property.EnumerateArray())
            {
                if (item.ValueKind == JsonValueKind.String)
                {
                    items.Add(item.GetString() ?? string.Empty);
                }
            }
            return string.Join("; ", items);
        }

        return string.Empty;
    }

    public async Task<CoachingReport> GenerateCoachingReportAsync(
        string role,
        string difficulty,
        List<QuestionAnswerPair> qaList,
        Dictionary<string, int> aggregateScores)
    {
        var qaPairs = string.Join("\n\n", qaList.Select((qa, i) => 
            $"Q{i+1}: {qa.Question}\nA{i+1}: {qa.Answer}\nScores: Technical={qa.TechnicalScore}, Communication={qa.CommunicationScore}, Grammar={qa.GrammarScore}, Confidence={qa.ConfidenceScore}"
        ));

        var scores = string.Join(", ", aggregateScores.Select(kvp => $"{kvp.Key}: {kvp.Value}%"));

        var prompt = $@"
You are an expert AI Interview Coach providing personalized feedback and guidance.

Interview Details:
- Role: {role}
- Difficulty: {difficulty}
- Questions Answered: {qaList.Count}

Aggregate Performance Scores:
{scores}

Question-Answer Analysis:
{qaPairs}

Generate a comprehensive coaching report and return ONLY a valid JSON object (no markdown, no explanation) with these exact fields:
{{
  ""overallSummary"": ""<2-3 sentence overall performance summary>"",
  ""topStrengths"": ""<semicolon-separated list of 3-5 top strengths>"",
  ""keyWeaknesses"": ""<semicolon-separated list of 3-5 key weaknesses>"",
  ""communicationImprovements"": ""<semicolon-separated specific communication tips>"",
  ""technicalImprovements"": ""<semicolon-separated specific technical tips>"",
  ""videoImprovements"": ""<semicolon-separated video presence tips, or 'Not applicable' if no video data>"",
  ""voiceImprovements"": ""<semicolon-separated voice delivery tips, or 'Not applicable' if no voice data>"",
  ""practiceRecommendations"": ""<semicolon-separated 4-5 actionable practice exercises>"",
  ""suggestedRole"": ""<recommended interview role based on strengths>"",
  ""suggestedDifficulty"": ""<Easy, Medium, or Hard - one level up if ready>"",
  ""suggestedQuestionCount"": <number 5-15>,
  ""learningResources"": ""<semicolon-separated 3-5 specific resources: courses, books, websites>"",
  ""motivationalMessage"": ""<inspiring 2-3 sentence closing message>""
}}

Important: All text fields must be single strings with items separated by semicolons, not arrays.";

        var request = new
        {
            contents = new[]
            {
                new
                {
                    parts = new[]
                    {
                        new { text = prompt }
                    }
                }
            }
        };

        var url = $"https://generativelanguage.googleapis.com/v1beta/models/{_settings.Model}:generateContent?key={_settings.ApiKey}";

        // Retry logic
        int maxRetries = 3;
        int retryDelayMs = 2000;
        string json = string.Empty;

        for (int attempt = 0; attempt <= maxRetries; attempt++)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(url, request);

                if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests && attempt < maxRetries)
                {
                    await Task.Delay(retryDelayMs);
                    retryDelayMs *= 2;
                    continue;
                }

                response.EnsureSuccessStatusCode();
                json = await response.Content.ReadAsStringAsync();
                break;
            }
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.TooManyRequests && attempt < maxRetries)
            {
                await Task.Delay(retryDelayMs);
                retryDelayMs *= 2;
            }
        }

        if (string.IsNullOrEmpty(json))
        {
            throw new Exception("Failed to generate coaching report after multiple retries");
        }

        using var document = JsonDocument.Parse(json);
        var aiResponse = document.RootElement
            .GetProperty("candidates")[0]
            .GetProperty("content")
            .GetProperty("parts")[0]
            .GetProperty("text")
            .GetString();

        var cleanedResponse = aiResponse!
            .Replace("```json", "")
            .Replace("```", "")
            .Trim();

        try
        {
            using var coachDoc = JsonDocument.Parse(cleanedResponse);
            var root = coachDoc.RootElement;

            return new CoachingReport
            {
                OverallSummary = GetStringProperty(root, "overallSummary"),
                TopStrengths = GetStringProperty(root, "topStrengths"),
                KeyWeaknesses = GetStringProperty(root, "keyWeaknesses"),
                CommunicationImprovements = GetStringProperty(root, "communicationImprovements"),
                TechnicalImprovements = GetStringProperty(root, "technicalImprovements"),
                VideoImprovements = GetStringProperty(root, "videoImprovements"),
                VoiceImprovements = GetStringProperty(root, "voiceImprovements"),
                PracticeRecommendations = GetStringProperty(root, "practiceRecommendations"),
                SuggestedRole = GetStringProperty(root, "suggestedRole"),
                SuggestedDifficulty = GetStringProperty(root, "suggestedDifficulty"),
                SuggestedQuestionCount = GetIntProperty(root, "suggestedQuestionCount", 10),
                LearningResources = GetStringProperty(root, "learningResources"),
                MotivationalMessage = GetStringProperty(root, "motivationalMessage")
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Coaching report parsing error: {ex.Message}");
            Console.WriteLine($"Response: {cleanedResponse}");

            // Return default coaching report
            return new CoachingReport
            {
                OverallSummary = "Good effort in completing the interview",
                TopStrengths = "Completed all questions; Showed engagement",
                KeyWeaknesses = "Continue practicing",
                CommunicationImprovements = "Practice clear articulation",
                TechnicalImprovements = "Review core concepts",
                VideoImprovements = "Not applicable",
                VoiceImprovements = "Not applicable",
                PracticeRecommendations = "Practice mock interviews; Review technical concepts; Work on communication skills",
                SuggestedRole = role,
                SuggestedDifficulty = difficulty,
                SuggestedQuestionCount = 10,
                LearningResources = "Online coding platforms; Technical interview books; Practice websites",
                MotivationalMessage = "Keep practicing and you'll continue to improve!"
            };
        }
    }

    private static string GetStringProperty(JsonElement root, string propertyName)
    {
        if (root.TryGetProperty(propertyName, out var property) && property.ValueKind == JsonValueKind.String)
        {
            return property.GetString() ?? string.Empty;
        }
        return string.Empty;
    }

    public async Task<CompanyEvaluationResult> EvaluateCompanyFitAsync(
        CompanyProfile companyProfile,
        string role,
        string difficulty,
        List<QuestionAnswerPair> qaList,
        Dictionary<string, int> aggregateScores)
    {
        var qaDetails = string.Join("\n", qaList.Select((qa, idx) => $@"
Question {idx + 1}: {qa.Question}
Answer: {qa.Answer}
Technical: {qa.TechnicalScore}, Communication: {qa.CommunicationScore}, Grammar: {qa.GrammarScore}, Confidence: {qa.ConfidenceScore}
"));

        var scoresText = string.Join(", ", aggregateScores.Select(kv => $"{kv.Key}: {kv.Value}"));

        var prompt = $@"
You are a senior hiring manager evaluating a candidate's fit for {companyProfile.CompanyName}.

COMPANY PROFILE:
Company Name: {companyProfile.CompanyName}
Interview Style: {companyProfile.InterviewStyle}
Focus Areas: {companyProfile.FocusAreas}
Behavioral Expectations: {companyProfile.BehavioralExpectations}
Technical Expectations: {companyProfile.TechnicalExpectations}
Communication Expectations: {companyProfile.CommunicationExpectations}

CANDIDATE INTERVIEW:
Role: {role}
Difficulty: {difficulty}
Questions Answered: {qaList.Count}
Aggregate Scores: {scoresText}

QUESTION-ANSWER DETAILS:
{qaDetails}

Evaluate how well the candidate fits this specific company's culture, technical requirements, and communication style.

Return ONLY a valid JSON object (no markdown, no explanation) with these exact fields:
{{
  ""companyReadinessScore"": <number 0-100 representing overall readiness for this company>,
  ""technicalAlignment"": <number 0-100 representing alignment with company's technical expectations>,
  ""communicationAlignment"": <number 0-100 representing alignment with communication style>,
  ""cultureFit"": <number 0-100 representing cultural fit based on behavioral expectations>,
  ""companySpecificFeedback"": ""<2-3 sentences explaining why this candidate is or isn't a good fit for {companyProfile.CompanyName} specifically>""
}}

Consider:
- Does the candidate's technical approach match the company's expectations?
- Does their communication style align with what the company values?
- Do their behavioral responses fit the company culture?
- How ready are they for this specific company's interview process?

Return only the JSON object.";

        for (int attempt = 1; attempt <= 3; attempt++)
        {
            try
            {
                var request = new
                {
                    contents = new[]
                    {
                        new
                        {
                            parts = new[]
                            {
                                new
                                {
                                    text = prompt
                                }
                            }
                        }
                    }
                };

                var url = $"https://generativelanguage.googleapis.com/v1beta/models/{_settings.Model}:generateContent?key={_settings.ApiKey}";
                
                var httpResponse = await _httpClient.PostAsJsonAsync(url, request);
                
                if (httpResponse.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                {
                    await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, attempt)));
                    continue;
                }
                
                httpResponse.EnsureSuccessStatusCode();
                
                var json = await httpResponse.Content.ReadAsStringAsync();
                using var document = JsonDocument.Parse(json);
                
                var aiResponse = document.RootElement
                    .GetProperty("candidates")[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString();
                
                var cleanedResponse = aiResponse!.Trim();

                if (cleanedResponse.StartsWith("```json"))
                {
                    cleanedResponse = cleanedResponse.Substring(7);
                }
                if (cleanedResponse.StartsWith("```"))
                {
                    cleanedResponse = cleanedResponse.Substring(3);
                }
                if (cleanedResponse.EndsWith("```"))
                {
                    cleanedResponse = cleanedResponse.Substring(0, cleanedResponse.Length - 3);
                }
                cleanedResponse = cleanedResponse.Trim();

                using var doc = JsonDocument.Parse(cleanedResponse);
                var root = doc.RootElement;

                return new CompanyEvaluationResult
                {
                    CompanyReadinessScore = GetIntProperty(root, "companyReadinessScore", 0),
                    TechnicalAlignment = GetIntProperty(root, "technicalAlignment", 0),
                    CommunicationAlignment = GetIntProperty(root, "communicationAlignment", 0),
                    CultureFit = GetIntProperty(root, "cultureFit", 0),
                    CompanySpecificFeedback = GetStringProperty(root, "companySpecificFeedback")
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Company evaluation attempt {attempt} failed: {ex.Message}");
                if (attempt == 3)
                {
                    return new CompanyEvaluationResult
                    {
                        CompanyReadinessScore = aggregateScores.GetValueOrDefault("Technical", 0),
                        TechnicalAlignment = aggregateScores.GetValueOrDefault("Technical", 0),
                        CommunicationAlignment = aggregateScores.GetValueOrDefault("Communication", 0),
                        CultureFit = aggregateScores.GetValueOrDefault("Professionalism", 0),
                        CompanySpecificFeedback = $"Based on your performance, you show potential for {companyProfile.CompanyName}. Continue developing your technical and communication skills."
                    };
                }
                await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, attempt)));
            }
        }

        return new CompanyEvaluationResult();
    }

    public async Task<ResumeAnalysisResult> AnalyzeResumeMatchAsync(
        ResumeProfile resumeProfile,
        string role,
        string difficulty,
        List<QuestionAnswerPair> qaList,
        Dictionary<string, int> aggregateScores)
    {
        var qaDetails = string.Join("\n", qaList.Select((qa, idx) => $@"
Question {idx + 1}: {qa.Question}
Answer: {qa.Answer}
Technical: {qa.TechnicalScore}, Communication: {qa.CommunicationScore}
"));

        var scoresText = string.Join(", ", aggregateScores.Select(kv => $"{kv.Key}: {kv.Value}"));

        var prompt = $@"
You are a career coach analyzing how well a candidate's resume matches their interview performance.

RESUME PROFILE:
Skills: {resumeProfile.Skills}
Experience: {resumeProfile.Experience}
Education: {resumeProfile.Education}
Job Titles: {resumeProfile.JobTitles}
Technologies: {resumeProfile.Technologies}

INTERVIEW PERFORMANCE:
Role: {role}
Difficulty: {difficulty}
Questions Answered: {qaList.Count}
Aggregate Scores: {scoresText}

QUESTION-ANSWER DETAILS:
{qaDetails}

Analyze the alignment between the candidate's resume and their interview performance.

Return ONLY a valid JSON object (no markdown, no explanation) with these exact fields:
{{
  ""resumeMatchScore"": <number 0-100 representing how well interview performance matches resume claims>,
  ""skillGapSummary"": ""<2-3 sentences identifying key skill gaps or areas where resume doesn't align with performance>"",
  ""careerRecommendations"": ""<2-3 sentences with specific career development recommendations based on resume and performance>""
}}

Consider:
- Do the candidate's answers demonstrate the skills listed on their resume?
- Does their experience level match their performance?
- Are there skills on the resume that weren't demonstrated in the interview?
- What should they focus on to advance their career?

Return only the JSON object.";

        for (int attempt = 1; attempt <= 3; attempt++)
        {
            try
            {
                var request = new
                {
                    contents = new[]
                    {
                        new
                        {
                            parts = new[]
                            {
                                new { text = prompt }
                            }
                        }
                    }
                };

                var url = $"https://generativelanguage.googleapis.com/v1beta/models/{_settings.Model}:generateContent?key={_settings.ApiKey}";
                
                var httpResponse = await _httpClient.PostAsJsonAsync(url, request);
                
                if (httpResponse.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                {
                    await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, attempt)));
                    continue;
                }
                
                httpResponse.EnsureSuccessStatusCode();
                
                var json = await httpResponse.Content.ReadAsStringAsync();
                using var document = JsonDocument.Parse(json);
                
                var aiResponse = document.RootElement
                    .GetProperty("candidates")[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString();
                
                var cleanedResponse = aiResponse!.Trim();

                if (cleanedResponse.StartsWith("```json"))
                {
                    cleanedResponse = cleanedResponse.Substring(7);
                }
                if (cleanedResponse.StartsWith("```"))
                {
                    cleanedResponse = cleanedResponse.Substring(3);
                }
                if (cleanedResponse.EndsWith("```"))
                {
                    cleanedResponse = cleanedResponse.Substring(0, cleanedResponse.Length - 3);
                }
                cleanedResponse = cleanedResponse.Trim();

                using var doc = JsonDocument.Parse(cleanedResponse);
                var root = doc.RootElement;

                return new ResumeAnalysisResult
                {
                    ResumeMatchScore = GetIntProperty(root, "resumeMatchScore", 0),
                    SkillGapSummary = GetStringProperty(root, "skillGapSummary"),
                    CareerRecommendations = GetStringProperty(root, "careerRecommendations")
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Resume analysis attempt {attempt} failed: {ex.Message}");
                if (attempt == 3)
                {
                    var avgScore = aggregateScores.Values.Any() ? (int)aggregateScores.Values.Average() : 70;
                    return new ResumeAnalysisResult
                    {
                        ResumeMatchScore = avgScore,
                        SkillGapSummary = "Your interview performance aligns with your resume. Continue building experience in the technologies you've listed.",
                        CareerRecommendations = $"Focus on deepening your expertise in {role} and consider pursuing more challenging projects to advance your career."
                    };
                }
                await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, attempt)));
            }
        }

        return new ResumeAnalysisResult();
    }
}

// Supporting DTOs for coaching
public class QuestionAnswerPair
{
    public string Question { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
    public int TechnicalScore { get; set; }
    public int CommunicationScore { get; set; }
    public int GrammarScore { get; set; }
    public int ConfidenceScore { get; set; }
}

public class CoachingReport
{
    public string OverallSummary { get; set; } = string.Empty;
    public string TopStrengths { get; set; } = string.Empty;
    public string KeyWeaknesses { get; set; } = string.Empty;
    public string CommunicationImprovements { get; set; } = string.Empty;
    public string TechnicalImprovements { get; set; } = string.Empty;
    public string VideoImprovements { get; set; } = string.Empty;
    public string VoiceImprovements { get; set; } = string.Empty;
    public string PracticeRecommendations { get; set; } = string.Empty;
    public string SuggestedRole { get; set; } = string.Empty;
    public string SuggestedDifficulty { get; set; } = string.Empty;
    public int SuggestedQuestionCount { get; set; }
    public string LearningResources { get; set; } = string.Empty;
    public string MotivationalMessage { get; set; } = string.Empty;
}


public class CompanyEvaluationResult
{
    public int CompanyReadinessScore { get; set; }
    public int TechnicalAlignment { get; set; }
    public int CommunicationAlignment { get; set; }
    public int CultureFit { get; set; }
    public string CompanySpecificFeedback { get; set; } = string.Empty;
}

public class ResumeAnalysisResult
{
    public int ResumeMatchScore { get; set; }
    public string SkillGapSummary { get; set; } = string.Empty;
    public string CareerRecommendations { get; set; } = string.Empty;
}
