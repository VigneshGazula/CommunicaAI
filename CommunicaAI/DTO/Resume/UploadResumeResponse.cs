namespace CommunicaAI.DTO.Resume
{
    public class UploadResumeResponse
    {
        public Guid ResumeId { get; set; }
        public string FileName { get; set; } = string.Empty;
        public ResumeMetadataDto Metadata { get; set; } = new();
    }

    public class ResumeMetadataDto
    {
        public List<string> Skills { get; set; } = new();
        public string Experience { get; set; } = string.Empty;
        public List<string> Education { get; set; } = new();
        public List<string> JobTitles { get; set; } = new();
        public List<string> Technologies { get; set; } = new();
        public string Summary { get; set; } = string.Empty;
    }

    public class ResumeProfileResponse
    {
        public Guid Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string Experience { get; set; } = string.Empty;
        public List<string> Skills { get; set; } = new();
        public DateTime UploadedAt { get; set; }
    }
}
