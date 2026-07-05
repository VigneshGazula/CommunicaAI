namespace CommunicaAI.DTO.Company
{
    public class CompanyProfileResponse
    {
        public Guid Id { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string InterviewStyle { get; set; } = string.Empty;
        public string FocusAreas { get; set; } = string.Empty;
    }
}
