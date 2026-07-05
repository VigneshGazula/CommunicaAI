namespace CommunicaAI.Models
{
    public class CompanyProfile
    {
        public Guid Id { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string InterviewStyle { get; set; } = string.Empty;
        public string FocusAreas { get; set; } = string.Empty;
        public string BehavioralExpectations { get; set; } = string.Empty;
        public string TechnicalExpectations { get; set; } = string.Empty;
        public string CommunicationExpectations { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }
    }
}
