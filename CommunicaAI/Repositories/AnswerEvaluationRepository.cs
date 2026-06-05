using CommunicaAI.Data;
using CommunicaAI.Models;
using CommunicaAI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

public class AnswerEvaluationRepository
    : IAnswerEvaluationRepository
{
    private readonly ApplicationDbContext _context;

    public AnswerEvaluationRepository(
        ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AnswerEvaluation?>
        GetByAnswerIdAsync(Guid answerId)
    {
        return await _context.AnswerEvaluations
            .FirstOrDefaultAsync(x =>
                x.InterviewAnswerId == answerId);
    }

    public async Task<AnswerEvaluation>
        CreateAsync(AnswerEvaluation evaluation)
    {
        _context.AnswerEvaluations.Add(evaluation);

        await _context.SaveChangesAsync();

        return evaluation;
    }
}