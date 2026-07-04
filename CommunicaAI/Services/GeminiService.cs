using System.Text;
using System.Text.Json;
using CommunicaAI.Configurations;
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
}