using CommunicaAI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CommunicaAI.Controllers;

[ApiController]
[Route("api/test")]
public class TestController : ControllerBase
{
    private readonly IGeminiService _geminiService;

    public TestController(
        IGeminiService geminiService)
    {
        _geminiService = geminiService;
    }

    [HttpGet("gemini")]
    public async Task<IActionResult> TestGemini()
    {
        var result =
            await _geminiService.EvaluateAnswerAsync(
                "What is Dependency Injection?",
                "Dependency Injection allows dependencies to be supplied from outside the class."
            );

        return Ok(result);
    }
}