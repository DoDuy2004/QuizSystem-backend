using QuizSystem_backend.Models;

namespace QuizSystem_backend.repositories
{
    public interface IQuestionBankRepository
    {
        Task<IEnumerable<QuestionBank>> GetQuestionBanksAsync();
        Task<QuestionBank> GetQuestionBankByIdAsync(Guid id);
        Task<QuestionBank> AddAsync(QuestionBank questionBank);
        Task<IEnumerable<Question>> GetQuestionsByQuestionBankAsync(Guid id);
        Task SaveChangesAsync();
    }
}
