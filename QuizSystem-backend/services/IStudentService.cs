using QuizSystem_backend.DTOs.ExamDtos;
using QuizSystem_backend.DTOs.StudentDtos;
using QuizSystem_backend.DTOs.StudentExamDto;
using QuizSystem_backend.Models;

namespace QuizSystem_backend.services
{
    public interface IStudentService
    {
        Task<IEnumerable<Student>> GetStudentsAsync();
        Task<Student> GetStudentByIdAsync(Guid id);
        Task<List<Student>> ImportStudentConfirm(List<StudentImportDto> studentsPreview);

        Task<List<StudentImportDto>> ImportFileStudentPreview(IFormFile file);
        Task<StudentExamResultDto?> SubmitStudentExamAsync(SubmitStudentExamDto dto);
        
    }
    
}
