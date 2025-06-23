using QuizSystem_backend.Models;

namespace QuizSystem_backend.repositories
{
    public interface IQuestionBankRepository
    {
        Task<IEnumerable<QuestionBank>> GetQuestionBanksAsync();
    }
}
