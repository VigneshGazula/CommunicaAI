using System.ComponentModel.DataAnnotations;

namespace CommunicaAI.DTO.Auth
{
    public class PasswordLoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
