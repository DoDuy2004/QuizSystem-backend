using QuizSystem_backend.Models;
using System.Linq.Expressions;

namespace QuizSystem_backend.repositories
{
    public interface IQuestionBankRepository
    {
        Task<IEnumerable<QuestionBank>> GetQuestionBanksAsync();
        Task<QuestionBank> GetQuestionBankByIdAsync(Guid id);
        Task<QuestionBank> AddAsync(QuestionBank questionBank);
        Task<IEnumerable<Question>> GetQuestionsByQuestionBankAsync(Guid id);
        Task SaveChangesAsync();
        Task AddQuestionAsync(Question newQuestion);
        Task <List<Question>>AddListQuestionAsync(List<Question> listQuestion);

        Task<string?> CheckErrorSubChap(string nameSubject, string nameChapter);
    }
}
