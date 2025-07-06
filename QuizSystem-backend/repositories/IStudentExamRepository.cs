using QuizSystem_backend.DTOs.StudentExamDto;
using QuizSystem_backend.Models;

namespace QuizSystem_backend.repositories
{
    public interface IStudentExamRepository
    {
        Task<StudentExamResultDto?> GradeStudentExamAsync(Guid studentExamId);
        Task<StudentExam?> AddStudentExamAsync(StudentExam studentExam);
    }
}
