using System.Text.Json;
using System.Text.Json.Serialization;

namespace CommunicaAI.Services;

public class VideoAnalysisService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<VideoAnalysisService> _logger;
    private readonly string _pythonServiceUrl;

    public VideoAnalysisService(
        HttpClient httpClient,
        ILogger<VideoAnalysisService> logger,
        IConfiguration configuration)
    {
        _httpClient = httpClient;
        _logger = logger;
        _pythonServiceUrl = configuration["VideoAnalysis:ServiceUrl"] ?? "http://localhost:8001";
    }

    public async Task<VideoMetrics?> AnalyzeFrameAsync(string base64Frame)
    {
        try
        {
            var request = new { frame = base64Frame };
            var response = await _httpClient.PostAsJsonAsync(
                $"{_pythonServiceUrl}/analyze-frame",
                request
            );

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Video analysis service returned {StatusCode}", response.StatusCode);
                return null;
            }

            var metrics = await response.Content.ReadFromJsonAsync<VideoMetrics>();
            return metrics;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling video analysis service");
            return null;
        }
    }

    public async Task<VideoAnalysisSummary?> GetSummaryAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_pythonServiceUrl}/summary");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Failed to get video analysis summary");
                return null;
            }

            var summary = await response.Content.ReadFromJsonAsync<VideoAnalysisSummary>();
            return summary;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting video analysis summary");
            return null;
        }
    }

    public async Task<bool> ResetAnalyzerAsync()
    {
        try
        {
            var response = await _httpClient.PostAsync($"{_pythonServiceUrl}/reset", null);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error resetting video analyzer");
            return false;
        }
    }

    public async Task<bool> CheckHealthAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_pythonServiceUrl}/health");
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}

// DTOs matching Python service responses
public class VideoMetrics
{
    [JsonPropertyName("timestamp")]
    public string Timestamp { get; set; } = string.Empty;

    [JsonPropertyName("faceDetected")]
    public bool FaceDetected { get; set; }

    [JsonPropertyName("eyeContactScore")]
    public double EyeContactScore { get; set; }

    [JsonPropertyName("headPoseScore")]
    public double HeadPoseScore { get; set; }

    [JsonPropertyName("smileDetected")]
    public bool SmileDetected { get; set; }

    [JsonPropertyName("emotionScore")]
    public double EmotionScore { get; set; }

    [JsonPropertyName("faceVisibility")]
    public double FaceVisibility { get; set; }

    [JsonPropertyName("confidenceScore")]
    public double ConfidenceScore { get; set; }
}

public class VideoAnalysisSummary
{
    [JsonPropertyName("averageEyeContact")]
    public double AverageEyeContact { get; set; }

    [JsonPropertyName("averagePosture")]
    public double AveragePosture { get; set; }

    [JsonPropertyName("averageExpression")]
    public double AverageExpression { get; set; }

    [JsonPropertyName("videoConfidenceScore")]
    public double VideoConfidenceScore { get; set; }

    [JsonPropertyName("totalFramesAnalyzed")]
    public int TotalFramesAnalyzed { get; set; }

    [JsonPropertyName("faceDetectionRate")]
    public double FaceDetectionRate { get; set; }

    [JsonPropertyName("feedback")]
    public string Feedback { get; set; } = string.Empty;
}
