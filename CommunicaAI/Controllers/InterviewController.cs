using CommunicaAI.DTO.Interview;
using CommunicaAI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CommunicaAI.Controllers
{
    [ApiController]
    [Route("api/interviews")]
    [Authorize]
    public class InterviewController : ControllerBase
    {
        private readonly IInterviewService _interviewService;
        private readonly IInterviewQuestionService _questionService;
        private readonly IInterviewAnswerService _answerService;

        public InterviewController(
            IInterviewService interviewService,
            IInterviewQuestionService questionService,
            IInterviewAnswerService answerService)
        {
            _interviewService = interviewService;
            _questionService = questionService;
            _answerService = answerService;
        }

        [HttpPost]
        public async Task<ActionResult<CreateInterviewResponse>> CreateInterview([FromBody] CreateInterviewRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { message = "Invalid token." });
            }

            var response = await _interviewService.CreateInterviewAsync(userId, request);

            return CreatedAtAction(
                nameof(GetInterview),
                new { sessionId = response.SessionId },
                response
            );
        }

        [HttpGet("{sessionId}")]
        public async Task<ActionResult<InterviewDetailResponse>> GetInterview(Guid sessionId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { message = "Invalid token." });
            }

            var response = await _interviewService.GetInterviewDetailsAsync(sessionId, userId);

            if (response == null)
            {
                return NotFound(new { message = "Interview session not found." });
            }

            return Ok(response);
        }

        [HttpGet("my-history")]
        public async Task<ActionResult<List<InterviewHistoryResponse>>> GetMyHistory()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { message = "Invalid token." });
            }

            var sessions = await _interviewService.GetUserHistoryAsync(userId);

            return Ok(sessions);
        }

        [HttpGet("{sessionId}/questions")]
        public async Task<ActionResult<List<QuestionResponse>>> GetSessionQuestions(Guid sessionId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { message = "Invalid token." });
            }

            var questions = await _questionService.GetSessionQuestionsAsync(sessionId, userId);

            if (!questions.Any())
            {
                return NotFound(new { message = "No questions found for this session." });
            }

            return Ok(questions);
        }

        [HttpPost("{sessionId}/answers")]
        public async Task<ActionResult<AnswerResponse>> SubmitAnswer(Guid sessionId, [FromBody] AnswerSubmitRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { message = "Invalid token." });
            }

            try
            {
                var response = await _answerService.SubmitAnswerAsync(sessionId, userId, request);
                return Ok(response);
            }
            catch (UnauthorizedAccessException)
            {
                return NotFound(new { message = "Session not found or unauthorized." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{sessionId}/complete")]
        public async Task<IActionResult> CompleteInterview(Guid sessionId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { message = "Invalid token." });
            }

            var success = await _interviewService.CompleteInterviewAsync(sessionId, userId);

            if (!success)
            {
                return NotFound(new { message = "Interview session not found." });
            }

            return Ok(new { message = "Interview completed successfully." });
        }
    }
}
