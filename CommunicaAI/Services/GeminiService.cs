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
You are a senior technical interviewer.

Question:
{question}

Candidate Answer:
{answer}

Evaluate the answer.

Return ONLY a JSON object with these fields:

technicalScore
clarityScore
completenessScore
overallScore
strengths
improvements
feedback

Do not include markdown.
Do not include explanation outside JSON.";

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

        return JsonSerializer.Deserialize<SubmitAudioAnswerResponse>(
            aiResponse!,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
    }
}