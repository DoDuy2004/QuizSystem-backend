using QuizSystem_backend.DTOs;
using QuizSystem_backend.Models;

namespace QuizSystem_backend.services
{
    public interface ICourseClassService
    {
        Task<IEnumerable<CourseClassDto>> GetCourseClassesAsync();
        Task<CourseClassDto> GetCourseClassByIdAsync(Guid id);
        Task<(bool success, string? message, object? data)> AddStudentToCourseAsync(StudentCourseClassDto scc);
        Task<IEnumerable<StudentDto>> GetStudentByCourseClassAsync(Guid id);
        Task<CourseClassDto> AddCourseClassAsync(CourseClassDto dto);
        Task<CourseClassDto> UpdateCourseClassAsync(Guid id, CourseClassDto dto);
        Task<bool> DeleteCourseClassAsync(Guid id);
    }
}
