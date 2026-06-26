using CommunicaAI.Data;
using CommunicaAI.Models;
using CommunicaAI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CommunicaAI.Repositories;

public class AnswerEvaluationRepository : IAnswerEvaluationRepository
{
    private readonly ApplicationDbContext _context;

    public AnswerEvaluationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AnswerEvaluation> CreateAsync(
        AnswerEvaluation evaluation)
    {
        _context.AnswerEvaluations.Add(evaluation);

        await _context.SaveChangesAsync();

        return evaluation;
    }

    public async Task<AnswerEvaluation?> GetByAnswerIdAsync(
        Guid answerId)
    {
        return await _context.AnswerEvaluations
            .FirstOrDefaultAsync(x =>
                x.InterviewAnswerId == answerId);
    }

    public async Task DeleteAsync(Guid id)
    {
        var evaluation = await _context.AnswerEvaluations.FindAsync(id);
        if (evaluation != null)
        {
            _context.AnswerEvaluations.Remove(evaluation);
            await _context.SaveChangesAsync();
        }
    }
}