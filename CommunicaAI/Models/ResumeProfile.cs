namespace CommunicaAI.Models
{
    public class ResumeProfile
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string ResumeUrl { get; set; } = string.Empty;
        public string FileType { get; set; } = string.Empty;
        
        // Parsed Resume Metadata (JSON stored as text)
        public string Skills { get; set; } = string.Empty; // Comma-separated
        public string Experience { get; set; } = string.Empty; // Years
        public string Education { get; set; } = string.Empty;
        public string JobTitles { get; set; } = string.Empty; // Comma-separated
        public string Technologies { get; set; } = string.Empty; // Comma-separated
        
        public DateTime UploadedAt { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
