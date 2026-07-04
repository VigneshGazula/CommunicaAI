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
You are a senior technical interviewer evaluating a candidate's answer.

Question:
{question}

Candidate Answer:
{answer}

Evaluate the answer and return ONLY a valid JSON object (no markdown, no explanation) with these exact fields:
{{
  ""technicalScore"": <number 0-100>,
  ""clarityScore"": <number 0-100>,
  ""completenessScore"": <number 0-100>,
  ""overallScore"": <number 0-100>,
  ""strengths"": ""<single string with strengths separated by semicolons>"",
  ""improvements"": ""<single string with improvements separated by semicolons>"",
  ""feedback"": ""<single string with overall feedback>""
}}

Important: strengths, improvements, and feedback must be single strings, not arrays.";

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

        var response =
            await _httpClient.PostAsJsonAsync(
                url,
                request);

        response.EnsureSuccessStatusCode();

        var json =
            await response.Content.ReadAsStringAsync();

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
                TechnicalScore = root.GetProperty("technicalScore").GetInt32(),
                ClarityScore = root.GetProperty("clarityScore").GetInt32(),
                CompletenessScore = root.GetProperty("completenessScore").GetInt32(),
                OverallScore = root.GetProperty("overallScore").GetInt32(),
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
                Strengths = "Answer provided",
                Improvements = "Could be more detailed",
                Feedback = "Evaluation could not be completed due to formatting issues"
            };
        }
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