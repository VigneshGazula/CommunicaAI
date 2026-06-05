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

        var response =
            await _httpClient.PostAsJsonAsync(
                url,
                requestBody);

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
}