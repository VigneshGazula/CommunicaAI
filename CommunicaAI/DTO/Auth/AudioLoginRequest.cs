using System.ComponentModel.DataAnnotations;

namespace CommunicaAI.DTO.Auth
{
    public class AudioLoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public IFormFile AudioFile { get; set; } = default!;
    }
}
