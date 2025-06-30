using QuizSystem_backend.DTOs;
using QuizSystem_backend.Enums;
using QuizSystem_backend.Models;
using static QuizSystem_backend.DTOs.ExamDto;

namespace QuizSystem_backend.repositories
{
    public interface IExamRepository
    {
        
        Task<IEnumerable<Exam>> GetExamsAsync();
        Task SaveChangesAsync();
        Task<Exam> GetExamByIdAsync(Guid examId);
        Task<Exam>GenerateAsync(Exam exam);
        Task<List<Question>> GetQuestionsByChapterAndDifficultyAsync(Guid chapterId, Difficulty difficulty, int take,Guid questionBankId);
        Task<Question> AddQuestionToExamAsync(Guid examId, Question question,float score);
    }
}
