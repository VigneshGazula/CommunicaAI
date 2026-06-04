using CommunicaAI.DTO.QuestionBank;

namespace CommunicaAI.Services.Interfaces
{
    public interface IQuestionBankService
    {
        Task<QuestionBankResponse> CreateQuestionAsync(CreateQuestionRequest request);
        Task<QuestionBankResponse?> GetQuestionByIdAsync(Guid id);
        Task<List<QuestionBankResponse>> GetAllQuestionsAsync();
        Task<bool> DeleteQuestionAsync(Guid id);
        Task SeedQuestionsAsync();
    }
}
