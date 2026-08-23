using CommunicaAI.Data;
using CommunicaAI.Models;
using CommunicaAI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace CommunicaAI.Repositories
{
    public class InterviewAnswerRepository : IInterviewAnswerRepository
    {
        private readonly ApplicationDbContext _context;

        public InterviewAnswerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<InterviewAnswer> CreateAsync(InterviewAnswer answer)
        {
            try
            {
                _context.InterviewAnswers.Add(answer);
                await _context.SaveChangesAsync();
                return answer;
            }
            catch (DbUpdateException ex) when (IsDuplicateInterviewQuestionConstraint(ex))
            {
                var existing = await _context.InterviewAnswers
                    .FirstOrDefaultAsync(a => a.InterviewQuestionId == answer.InterviewQuestionId);

                if (existing == null)
                {
                    throw;
                }

                existing.Transcript = answer.Transcript;
                existing.AudioUrl = answer.AudioUrl;
                existing.DurationSeconds = answer.DurationSeconds;
                existing.AnsweredAt = answer.AnsweredAt;
                existing.InterviewSessionId = answer.InterviewSessionId;

                await _context.SaveChangesAsync();
                return existing;
            }
        }

        private static bool IsDuplicateInterviewQuestionConstraint(DbUpdateException ex)
        {
            return ex.InnerException is PostgresException postgresException &&
                   postgresException.SqlState == PostgresErrorCodes.UniqueViolation &&
                   postgresException.ConstraintName == "IX_InterviewAnswers_InterviewQuestionId";
        }

        public async Task<InterviewAnswer?> GetByQuestionIdAsync(Guid questionId)
        {
            return await _context.InterviewAnswers
                .FirstOrDefaultAsync(a => a.InterviewQuestionId == questionId);
        }

        public async Task<List<InterviewAnswer>> GetBySessionIdAsync(Guid sessionId)
        {
            return await _context.InterviewAnswers
                .Where(a => a.InterviewSessionId == sessionId)
                .OrderBy(a => a.AnsweredAt)
                .ToListAsync();
        }

        public async Task UpdateAsync(InterviewAnswer answer)
        {
            _context.InterviewAnswers.Update(answer);
            await _context.SaveChangesAsync();
        }
    }
}
