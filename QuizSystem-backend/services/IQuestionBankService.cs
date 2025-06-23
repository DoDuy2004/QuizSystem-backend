using QuizSystem_backend.DTOs;

namespace QuizSystem_backend.services
{
    public interface IQuestionBankService
    {
        Task<IEnumerable<QuestionBankDto>> GetQuestionBanksAsync();
    }
}
