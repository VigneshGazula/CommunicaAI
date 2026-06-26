using CommunicaAI.DTO.QuestionBank;
using CommunicaAI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CommunicaAI.Controllers
{
    [ApiController]
    [Route("api/question-bank")]
    [Authorize]
    public class QuestionBankController : ControllerBase
    {
        private readonly IQuestionBankService _questionBankService;

        public QuestionBankController(IQuestionBankService questionBankService)
        {
            _questionBankService = questionBankService;
        }

        [HttpPost]
        public async Task<ActionResult<QuestionBankResponse>> CreateQuestion([FromBody] CreateQuestionRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _questionBankService.CreateQuestionAsync(request);
            return CreatedAtAction(nameof(GetQuestion), new { id = response.Id }, response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<QuestionBankResponse>> GetQuestion(Guid id)
        {
            var response = await _questionBankService.GetQuestionByIdAsync(id);
            
            if (response == null)
            {
                return NotFound(new { message = "Question not found" });
            }

            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult<List<QuestionBankResponse>>> GetAllQuestions()
        {
            var questions = await _questionBankService.GetAllQuestionsAsync();
            return Ok(questions);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuestion(Guid id)
        {
            var success = await _questionBankService.DeleteQuestionAsync(id);
            
            if (!success)
            {
                return NotFound(new { message = "Question not found" });
            }

            return Ok(new { message = "Question deleted successfully" });
        }

        [HttpPost("seed")]
        [AllowAnonymous]
        public async Task<IActionResult> SeedQuestions()
        {
            await _questionBankService.SeedQuestionsAsync();
            return Ok(new { message = "Questions seeded successfully" });
        }

        [HttpGet("metadata")]
        [AllowAnonymous]
        public async Task<ActionResult<InterviewMetadataResponse>> GetMetadata()
        {
            var metadata = await _questionBankService.GetMetadataAsync();
            return Ok(metadata);
        }
    }
}
