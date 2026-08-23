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
        try
        {
            using var ms = new MemoryStream();

            await audioStream.CopyToAsync(ms);

            var audioBytes = ms.ToArray();
            
            Console.WriteLine($"Audio transcription - Size: {audioBytes.Length} bytes, ContentType: {contentType}");

            if (audioBytes.Length == 0)
            {
                throw new Exception("Audio file is empty");
            }

            // Validate content type
            var validContentTypes = new[] { "audio/webm", "audio/mp4", "audio/wav", "audio/mpeg", "audio/ogg" };
            if (!validContentTypes.Contains(contentType.ToLower()))
            {
                Console.WriteLine($"Warning: Unusual content type: {contentType}. Attempting transcription anyway.");
            }

            var base64Audio = Convert.ToBase64String(audioBytes);

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
                    Console.WriteLine($"Transcription attempt {attempt + 1}/{maxRetries + 1}");
                    
                    var response =
                        await _httpClient.PostAsJsonAsync(
                            url,
                            requestBody);

                    if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests && attempt < maxRetries)
                    {
                        Console.WriteLine($"Rate limited. Retrying in {retryDelayMs}ms");
                        await Task.Delay(retryDelayMs);
                        retryDelayMs *= 2; // Double the delay for next attempt
                        continue;
                    }

                    var responseContent = await response.Content.ReadAsStringAsync();
                    
                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"Gemini API error: {response.StatusCode}");
                        Console.WriteLine($"Response: {responseContent}");
                        throw new Exception($"Gemini API returned {response.StatusCode}: {responseContent}");
                    }

                    using var doc = JsonDocument.Parse(responseContent);
                    
                    var transcript = doc.RootElement
                        .GetProperty("candidates")[0]
                        .GetProperty("content")
                        .GetProperty("parts")[0]
                        .GetProperty("text")
                        .GetString()!
                        .Trim();
                    
                    Console.WriteLine($"Transcription successful: {transcript.Substring(0, Math.Min(50, transcript.Length))}...");
                    
                    return transcript;
                }
                catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.TooManyRequests && attempt < maxRetries)
                {
                    Console.WriteLine($"Rate limited (HTTP exception). Retrying in {retryDelayMs}ms");
                    await Task.Delay(retryDelayMs);
                    retryDelayMs *= 2;
                }
                catch (Exception ex) when (attempt < maxRetries && 
                    (ex.Message.Contains("429") || ex.Message.Contains("rate limit")))
                {
                    Console.WriteLine($"Rate limited (general exception). Retrying in {retryDelayMs}ms");
                    await Task.Delay(retryDelayMs);
                    retryDelayMs *= 2;
                }
            }

            throw new Exception("Failed to transcribe audio after multiple retries. Please check your Gemini API key and quota.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Transcription failed: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            throw new Exception($"Audio transcription failed: {ex.Message}", ex);
        }
    }
}