using System.ComponentModel.DataAnnotations;

namespace CommunicaAI.DTO.Interview
{
    public class AnswerSubmitRequest
    {
        [Required]
        public Guid QuestionId { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(5000)]
        public string Transcript { get; set; } = string.Empty;
    }
}
