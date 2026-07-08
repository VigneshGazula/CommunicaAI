using System.ComponentModel.DataAnnotations;

namespace CommunicaAI.DTO.Interview
{
    public class CreateInterviewRequest
    {
        [Required(ErrorMessage = "Role is required")]
        public string Role { get; set; } = string.Empty;

        [Required(ErrorMessage = "Topic is required")]
        public string Topic { get; set; } = string.Empty;

        [Required(ErrorMessage = "Difficulty is required")]
        public string Difficulty { get; set; } = string.Empty;

        [Required]
        [Range(1, 50, ErrorMessage = "Question count must be between 1 and 50")]
        public int QuestionCount { get; set; }

        [Required]
        [Range(1, 180, ErrorMessage = "Duration must be between 1 and 180 minutes")]
        public int DurationMinutes { get; set; }

        // Module 6: Company Intelligence (optional)
        public Guid? CompanyProfileId { get; set; }

        // Module 7: Resume Intelligence (optional)
        public Guid? ResumeProfileId { get; set; }

        // Module 9: Specialized Interview Modes (optional, defaults to Technical)
        public string? InterviewType { get; set; }
    }
}
