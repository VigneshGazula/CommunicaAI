using System.ComponentModel.DataAnnotations;

namespace CommunicaAI.DTO.QuestionBank
{
    public class CreateQuestionRequest
    {
        [Required]
        [MaxLength(100)]
        public string Role { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Category { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Difficulty { get; set; } = string.Empty;

        [Required]
        [MaxLength(1000)]
        public string QuestionText { get; set; } = string.Empty;
    }
}
