using CommunicaAI.Models;
using Microsoft.AspNetCore.Http;

namespace CommunicaAI.Services;

public interface IPythonVerificationService
{
    Task<PythonVerificationResult> VerifyAudioAsync(
        string enrolledAudioUrl,
        IFormFile sampleAudio,
        CancellationToken cancellationToken = default);
}