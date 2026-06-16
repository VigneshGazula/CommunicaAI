using CommunicaAI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/interviews")]
[Authorize]
public class InterviewAnswerController : ControllerBase
{
    private readonly IInterviewAnswerService _answerService;

    public InterviewAnswerController(
        IInterviewAnswerService answerService)
    {
        _answerService = answerService;
    }

    [HttpPost("{sessionId}/answers/audio")]
    public async Task<IActionResult> SubmitAudioAnswer(
        Guid sessionId,
        [FromForm] Guid questionId,
        [FromForm] IFormFile audioFile,
        [FromForm] int durationSeconds)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(userIdClaim))
        {
            return Unauthorized();
        }

        var userId = Guid.Parse(userIdClaim);

        var result =
            await _answerService.SubmitAudioAnswerAsync(
                sessionId,
                questionId,
                audioFile,
                durationSeconds,
                userId);

        return Ok(result);
    }
}