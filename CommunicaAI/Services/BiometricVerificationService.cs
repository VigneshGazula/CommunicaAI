using CommunicaAI.Models;

namespace CommunicaAI.Services
{
    public class BiometricVerificationService : IBiometricVerificationService
    {
        public Task<bool> VerifyAudioAsync(UserVerificationProfile profile, IFormFile sampleAudio)
        {
            if (profile == null || sampleAudio == null || sampleAudio.Length == 0)
                return Task.FromResult(false);

            // Temporary development stub.
            // Replace with Python/AI feature matching later.
            return Task.FromResult(true);
        }

        public Task<bool> VerifyVideoAsync(UserVerificationProfile profile, IFormFile sampleVideo)
        {
            if (profile == null || sampleVideo == null || sampleVideo.Length == 0)
                return Task.FromResult(false);

            // Temporary development stub.
            // Replace with Python/AI feature matching later.
            return Task.FromResult(true);
        }
    }
}
