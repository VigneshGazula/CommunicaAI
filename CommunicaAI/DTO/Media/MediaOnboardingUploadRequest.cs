using System.ComponentModel.DataAnnotations;

namespace CommunicaAI.DTO.Media
{
    public class MediaOnboardingUploadRequest
    {
        [Required]
        public IFormFile AudioFile { get; set; } = default!;

        [Required]
        public IFormFile VideoFile { get; set; } = default!;
    }
}
