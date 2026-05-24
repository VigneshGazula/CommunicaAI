namespace CommunicaAI.Models;

public class PythonVerificationServiceOptions
{
    public string BaseUrl { get; set; } = string.Empty;
    public string VerifyAudioPath { get; set; } = "/verify-audio";
}