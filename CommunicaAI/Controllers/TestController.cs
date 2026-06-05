using CommunicaAI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CommunicaAI.Controllers;

[ApiController]
[Route("api/test")]
public class TestController : ControllerBase
{
    private readonly ITranscriptionService _transcriptionService;

    public TestController(
        ITranscriptionService transcriptionService)
    {
        _transcriptionService = transcriptionService;
    }

    [HttpPost("transcribe")]
    public async Task<IActionResult> Transcribe(
        IFormFile audioFile)
    {
        if (audioFile == null || audioFile.Length == 0)
        {
            return BadRequest("Audio file required.");
        }

        using var stream =
            audioFile.OpenReadStream();

        var transcript =
            await _transcriptionService
                .TranscribeAsync(
                    stream,
                    audioFile.ContentType);

        return Ok(new
        {
            Transcript = transcript
        });
    }
}