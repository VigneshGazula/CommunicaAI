namespace CommunicaAI.DTO.Interview;

public class InterviewTypesResponse
{
    public List<InterviewTypeInfo> InterviewTypes { get; set; } = new();
}

public class InterviewTypeInfo
{
    public string Type { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public List<string> FocusAreas { get; set; } = new();
}
