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
        var userId = Guid.Parse(
            User.FindFirst("UserId")!.Value);

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