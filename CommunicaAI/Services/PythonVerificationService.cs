using CommunicaAI.Models;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text.Json;

namespace CommunicaAI.Services
{
    public class PythonVerificationService : IPythonVerificationService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly PythonVerificationServiceOptions _options;

        public PythonVerificationService(
            IHttpClientFactory httpClientFactory,
            IOptions<PythonVerificationServiceOptions> options)
        {
            _httpClientFactory = httpClientFactory;
            _options = options.Value;
        }
        public async Task<PythonVerificationResult> VerifyAudioAsync(string enrolledAudioUrl, IFormFile sampleAudio, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(enrolledAudioUrl))
                throw new ArgumentException("Enrolled audio URL is missing.");

            if (sampleAudio == null || sampleAudio.Length == 0)
                throw new ArgumentException("Sample audio file is missing.");

            // Download enrolled audio from Cloudinary
            var downloadClient = _httpClientFactory.CreateClient();
            var enrolledBytes = await downloadClient.GetByteArrayAsync(enrolledAudioUrl, cancellationToken);

            var pythonClient = _httpClientFactory.CreateClient("PythonVerification");
            using var form = new MultipartFormDataContent();

            var enrolledContent = new ByteArrayContent(enrolledBytes);
            enrolledContent.Headers.ContentType = new MediaTypeHeaderValue("audio/wav");
            form.Add(enrolledContent, "enrolled_audio", "enrolled.wav");

            await using var sampleStream = sampleAudio.OpenReadStream();
            var sampleContent = new StreamContent(sampleStream);
            sampleContent.Headers.ContentType = new MediaTypeHeaderValue(
                string.IsNullOrWhiteSpace(sampleAudio.ContentType)
                    ? "audio/wav"
                    : sampleAudio.ContentType);
            form.Add(sampleContent, "sample_audio", sampleAudio.FileName);

            using var response = await pythonClient.PostAsync(
                _options.VerifyAudioPath,
                form,
                cancellationToken);

            var responseText = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(
                    $"Python verification failed: {(int)response.StatusCode} {responseText}");

            var result = JsonSerializer.Deserialize<PythonVerificationResult>(
                responseText,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return result ?? new PythonVerificationResult
            {
                Verified = false,
                Score = 0
            };
        }
    }
}
