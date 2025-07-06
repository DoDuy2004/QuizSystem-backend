using QuizSystem_backend.DTOs;
using QuizSystem_backend.Models;

namespace QuizSystem_backend.services
{
    public interface IQuestionBankService
    {
        Task<IEnumerable<QuestionBankDto>> GetQuestionBanksAsync(Guid userId);
        Task<QuestionBankDto> GetQuestionBankByIdAsync(Guid id);

        Task<QuestionBankDto> AddQuestionBankAsync(QuestionBankDto questionBank);
        Task<QuestionBankDto> UpdateQuestionBankAsync(Guid id ,QuestionBankDto questionBank);
        Task<IEnumerable<QuestionDto>> GetQuestionsByQuestionBankAsync(Guid id);
        Task<bool> DeleteQuestionBankAsync(Guid id);
       
        Task<List<QuestionImportPreviewDto>> ImportQuestionsPreview(IFormFile file);
        Task<List<Question>> ImPortQuestionConfirm(Guid id, List<QuestionImportPreviewDto> listPreview);

    }
}
