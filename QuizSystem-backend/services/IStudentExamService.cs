
using QuizSystem_backend.DTOs.StudentExamDto;

namespace QuizSystem_backend.services
{
    public interface IStudentExamService
    {
        Task<StudentExamResultDto> GetStudentExamResult(Guid studentExamId);
    }
}
