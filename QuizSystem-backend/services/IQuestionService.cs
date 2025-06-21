using QuizSystem_backend.DTOs;
using QuizSystem_backend.Models;

namespace QuizSystem_backend.services
{
    public interface IQuestionService
    {
        Task<IEnumerable<QuestionDto>> GetQuestionsAsync();
        Task<QuestionDto> AddQuestionAsync(QuestionDto dto);
        Task<QuestionDto> UpdateQuestionAsync(Guid id, QuestionDto dto);
        Task<QuestionDto> GetQuestionByIdAsync(Guid id);
    };
}
