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

        public InterviewController(IInterviewService interviewService)
        {
            _interviewService = interviewService;
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
        public async Task<ActionResult<InterviewSessionResponse>> GetInterview(Guid sessionId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { message = "Invalid token." });
            }

            var response = await _interviewService.GetInterviewAsync(sessionId, userId);

            if (response == null)
            {
                return NotFound(new { message = "Interview session not found." });
            }

            return Ok(response);
        }

        [HttpGet("my-history")]
        public async Task<ActionResult<List<InterviewSessionResponse>>> GetMyHistory()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { message = "Invalid token." });
            }

            var sessions = await _interviewService.GetUserInterviewsAsync(userId);

            return Ok(sessions);
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
