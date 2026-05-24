using System.ComponentModel.DataAnnotations;

namespace CommunicaAI.DTO.Auth
{
    public class VideoLoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public IFormFile VideoFile { get; set; } = default!;
    }
}
