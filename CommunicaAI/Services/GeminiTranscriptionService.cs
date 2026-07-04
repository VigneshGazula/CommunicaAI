using System.Text.Json;
using CommunicaAI.Configurations;
using CommunicaAI.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace CommunicaAI.Services;

public class GeminiTranscriptionService
    : ITranscriptionService
{
    private readonly HttpClient _httpClient;
    private readonly GeminiSettings _settings;

    public GeminiTranscriptionService(
        HttpClient httpClient,
        IOptions<GeminiSettings> options)
    {
        _httpClient = httpClient;
        _settings = options.Value;
    }

    public async Task<string> TranscribeAsync(
        Stream audioStream,
        string contentType)
    {
        using var ms = new MemoryStream();

        await audioStream.CopyToAsync(ms);

        var audioBytes = ms.ToArray();

        var base64Audio =
            Convert.ToBase64String(audioBytes);

        var requestBody = new
        {
            contents = new[]
            {
                new
                {
                    parts = new object[]
                    {
                        new
                        {
                            text =
                            "Transcribe the following interview answer. Return only the transcript text. Do not add explanations."
                        },
                        new
                        {
                            inline_data = new
                            {
                                mime_type = contentType,
                                data = base64Audio
                            }
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

        for (int attempt = 0; attempt <= maxRetries; attempt++)
        {
            try
            {
                var response =
                    await _httpClient.PostAsJsonAsync(
                        url,
                        requestBody);

                if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests && attempt < maxRetries)
                {
                    // Rate limited - wait and retry with exponential backoff
                    await Task.Delay(retryDelayMs);
                    retryDelayMs *= 2; // Double the delay for next attempt
                    continue;
                }

                response.EnsureSuccessStatusCode();

                var json =
                    await response.Content.ReadAsStringAsync();

                using var doc =
                    JsonDocument.Parse(json);

                return doc.RootElement
                    .GetProperty("candidates")[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString()!
                    .Trim();
            }
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.TooManyRequests && attempt < maxRetries)
            {
                // Rate limited - wait and retry with exponential backoff
                await Task.Delay(retryDelayMs);
                retryDelayMs *= 2;
            }
        }

        throw new Exception("Failed to transcribe audio after multiple retries due to rate limiting. Please try again later.");
    }
}