using System.Text.Json;

namespace CommunicaAI.Services
{
    public class ResumeParserService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ResumeParserService> _logger;

        public ResumeParserService(
            HttpClient httpClient,
            IConfiguration configuration,
            ILogger<ResumeParserService> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<ResumeMetadata?> ParseResumeAsync(Stream fileStream, string fileName)
        {
            try
            {
                var serviceUrl = _configuration["ResumeParser:ServiceUrl"] ?? "http://localhost:8002";
                
                using var content = new MultipartFormDataContent();
                var fileContent = new StreamContent(fileStream);
                
                // Set content type based on file extension
                var extension = Path.GetExtension(fileName).ToLowerInvariant();
                fileContent.Headers.ContentType = extension == ".pdf"
                    ? new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf")
                    : new System.Net.Http.Headers.MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.wordprocessingml.document");
                
                content.Add(fileContent, "file", fileName);

                var response = await _httpClient.PostAsync($"{serviceUrl}/parse-resume", content);
                
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Resume parser returned {response.StatusCode}");
                    return null;
                }

                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<ResumeParserResponse>(json, new JsonSerializerOptions 
                { 
                    PropertyNameCaseInsensitive = true 
                });

                if (result?.Success == true && result.Metadata != null)
                {
                    return result.Metadata;
                }

                _logger.LogError($"Resume parsing failed: {result?.Error}");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error calling resume parser: {ex.Message}");
                return null;
            }
        }
    }

    public class ResumeParserResponse
    {
        public bool Success { get; set; }
        public ResumeMetadata? Metadata { get; set; }
        public string? Error { get; set; }
    }

    public class ResumeMetadata
    {
        public List<string> Skills { get; set; } = new();
        public string Experience { get; set; } = string.Empty;
        public List<string> Education { get; set; } = new();
        public List<string> JobTitles { get; set; } = new();
        public List<string> Technologies { get; set; } = new();
        public string Summary { get; set; } = string.Empty;
    }
}
