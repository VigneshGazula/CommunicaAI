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

        [HttpGet("types")]
        [AllowAnonymous]
        public ActionResult<InterviewTypesResponse> GetInterviewTypes()
        {
            var interviewTypes = new List<InterviewTypeInfo>
            {
                new InterviewTypeInfo
                {
                    Type = "Technical",
                    DisplayName = "Technical",
                    Description = "Focuses on technical skills, problem-solving, and domain knowledge",
                    Icon = "💻",
                    FocusAreas = new List<string> { "Coding", "Algorithms", "System Knowledge", "Best Practices" }
                },
                new InterviewTypeInfo
                {
                    Type = "HR",
                    DisplayName = "HR",
                    Description = "Assesses cultural fit, work style, and interpersonal skills",
                    Icon = "👥",
                    FocusAreas = new List<string> { "Culture Fit", "Work Style", "Team Collaboration", "Career Goals" }
                },
                new InterviewTypeInfo
                {
                    Type = "Behavioral",
                    DisplayName = "Behavioral",
                    Description = "Evaluates past experiences and situational responses",
                    Icon = "🧠",
                    FocusAreas = new List<string> { "STAR Method", "Past Experiences", "Conflict Resolution", "Leadership" }
                },
                new InterviewTypeInfo
                {
                    Type = "Coding",
                    DisplayName = "Coding",
                    Description = "Live coding challenges and algorithmic problem-solving",
                    Icon = "⌨️",
                    FocusAreas = new List<string> { "Data Structures", "Algorithms", "Code Quality", "Optimization" }
                },
                new InterviewTypeInfo
                {
                    Type = "System Design",
                    DisplayName = "System Design",
                    Description = "Architectural design and scalability discussions",
                    Icon = "🏗️",
                    FocusAreas = new List<string> { "Architecture", "Scalability", "Trade-offs", "Distributed Systems" }
                },
                new InterviewTypeInfo
                {
                    Type = "DevOps",
                    DisplayName = "DevOps",
                    Description = "CI/CD, infrastructure, and deployment practices",
                    Icon = "🔧",
                    FocusAreas = new List<string> { "CI/CD", "Infrastructure", "Monitoring", "Automation" }
                },
                new InterviewTypeInfo
                {
                    Type = "Cloud",
                    DisplayName = "Cloud",
                    Description = "Cloud platforms, services, and architecture",
                    Icon = "☁️",
                    FocusAreas = new List<string> { "AWS/Azure/GCP", "Cloud Services", "Cost Optimization", "Security" }
                },
                new InterviewTypeInfo
                {
                    Type = "Data Science",
                    DisplayName = "Data Science",
                    Description = "Statistics, ML algorithms, and data analysis",
                    Icon = "📊",
                    FocusAreas = new List<string> { "Statistics", "ML Algorithms", "Data Analysis", "Feature Engineering" }
                },
                new InterviewTypeInfo
                {
                    Type = "AI/ML",
                    DisplayName = "AI/ML",
                    Description = "Machine learning, deep learning, and AI systems",
                    Icon = "🤖",
                    FocusAreas = new List<string> { "Neural Networks", "Model Training", "NLP", "Computer Vision" }
                },
                new InterviewTypeInfo
                {
                    Type = "Cyber Security",
                    DisplayName = "Cyber Security",
                    Description = "Security practices, threat analysis, and compliance",
                    Icon = "🔒",
                    FocusAreas = new List<string> { "Security Practices", "Threat Analysis", "Compliance", "Penetration Testing" }
                },
                new InterviewTypeInfo
                {
                    Type = "Product Manager",
                    DisplayName = "Product Manager",
                    Description = "Product strategy, roadmaps, and stakeholder management",
                    Icon = "📱",
                    FocusAreas = new List<string> { "Product Strategy", "Roadmaps", "User Research", "Metrics" }
                },
                new InterviewTypeInfo
                {
                    Type = "Solution Architect",
                    DisplayName = "Solution Architect",
                    Description = "Enterprise architecture and solution design",
                    Icon = "🏛️",
                    FocusAreas = new List<string> { "Enterprise Architecture", "Solution Design", "Integration", "Patterns" }
                }
            };

            return Ok(new InterviewTypesResponse { InterviewTypes = interviewTypes });
        }
    }
}
