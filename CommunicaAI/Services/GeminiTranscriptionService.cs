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
                                "Listen to the audio and provide ONLY the exact words spoken. Do not add any commentary, analysis, or additional text. Just transcribe what you hear word-for-word."
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
            int maxRetries = 5;
            int retryDelayMs = 3000; // Start with 3 seconds (longer initial delay)

            for (int attempt = 0; attempt <= maxRetries; attempt++)
            {
                try
                {
                    Console.WriteLine($"Transcription attempt {attempt + 1}/{maxRetries + 1}");
                    
                    var response =
                        await _httpClient.PostAsJsonAsync(
                            url,
                            requestBody);

                    if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                    {
                        if (attempt < maxRetries)
                        {
                            // Calculate exponential backoff with jitter
                            var jitter = Random.Shared.Next(0, 1000);
                            var totalDelay = retryDelayMs + jitter;
                            Console.WriteLine($"Rate limited (429). Retrying in {totalDelay}ms (attempt {attempt + 1}/{maxRetries})");
                            await Task.Delay(totalDelay);
                            retryDelayMs *= 2; // Double the delay for next attempt
                            continue;
                        }
                        else
                        {
                            throw new Exception("Gemini API rate limit exceeded. Please wait a moment and try again, or upgrade your API quota at https://aistudio.google.com/");
                        }
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
                    var jitter = Random.Shared.Next(0, 1000);
                    var totalDelay = retryDelayMs + jitter;
                    Console.WriteLine($"Rate limited (HTTP exception). Retrying in {totalDelay}ms");
                    await Task.Delay(totalDelay);
                    retryDelayMs *= 2;
                }
                catch (Exception ex) when (attempt < maxRetries && 
                    (ex.Message.Contains("429") || ex.Message.Contains("rate limit") || ex.Message.Contains("Too Many Requests")))
                {
                    var jitter = Random.Shared.Next(0, 1000);
                    var totalDelay = retryDelayMs + jitter;
                    Console.WriteLine($"Rate limited (general exception). Retrying in {totalDelay}ms");
                    await Task.Delay(totalDelay);
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