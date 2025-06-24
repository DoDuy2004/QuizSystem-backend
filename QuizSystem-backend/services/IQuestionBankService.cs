using QuizSystem_backend.DTOs;

namespace QuizSystem_backend.services
{
    public interface IQuestionBankService
    {
        Task<IEnumerable<QuestionBankDto>> GetQuestionBanksAsync();
        Task<QuestionBankDto> GetQuestionBankByIdAsync(Guid id);

        Task<QuestionBankDto> AddQuestionBankAsync(QuestionBankDto questionBank);
        Task<QuestionBankDto> UpdateQuestionBankAsync(Guid id ,QuestionBankDto questionBank);
        Task<IEnumerable<QuestionDto>> GetQuestionsByQuestionBankAsync(Guid id);
        Task<bool> DeleteQuestionBankAsync(Guid id);
    }
}
