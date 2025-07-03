using QuizSystem_backend.DTOs.StudentDtos;
using QuizSystem_backend.Models;

namespace QuizSystem_backend.services
{
    public interface IStudentService
    {
        Task<IEnumerable<Student>> GetStudentsAsync();
        Task<Student> GetStudentByIdAsync(Guid id);
        //Task<bool> ImportStudentConfirm(List<StudentImportDto> studentsImportPreviewDto);
        //Task<List<StudentImportDto>> ImportFileStudentPreview(IFormFile file);
    }
}
