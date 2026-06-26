namespace CommunicaAI.DTO.QuestionBank
{
    public class InterviewMetadataResponse
    {
        public List<string> Roles { get; set; } = new();
        public List<string> Difficulties { get; set; } = new();
        public List<string> Categories { get; set; } = new();
    }
}
