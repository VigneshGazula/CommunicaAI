using CommunicaAI.Models;

namespace CommunicaAI.Services
{
    public interface IBiometricVerificationService
    {
        Task<bool> VerifyAudioAsync(UserVerificationProfile profile, IFormFile sampleAudio);
        Task<bool> VerifyVideoAsync(UserVerificationProfile profile, IFormFile sampleVideo);
    }
}
