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
        Task<Exam?> GetExamByIdAsync(Guid examId);
        Task<Exam>GenerateAsync(Exam exam);
<<<<<<< HEAD
        Task<Exam> AddQuestionToExamAsync(Guid examId, Question questionDto);
        Task<List<Question>> GetQuestionsByChapterAndDifficultyAsync(Guid chapterId, Difficulty difficulty, int take,Guid questionBankId);
=======
        Task<Question> AddQuestionToExamAsync(Guid examId, Question question,float score);
        Task<List<Question>> GetQuestionsByChapterAndDifficultyAsync(Guid chapterId, string difficulty, int take,Guid questionBankId);
        
>>>>>>> d918e9e032bc963798241c3074f922c28d33c69a

    }
}
