using Microsoft.AspNetCore.Mvc;
using QuizSystem_backend.DTOs;
using QuizSystem_backend.Models;

namespace QuizSystem_backend.repositories
{
    public interface IQuestionRepository
    {
        Task<IEnumerable<Question>> GetQuestionsAsync();
        Task<Question> AddAsync(Question question);
        Task SaveChangesAsync();
        Task<Question> GetByIdAsync(Guid id);
    }
}
