using QuizSystem_backend.DTOs;
using QuizSystem_backend.DTOs.CourseClassDtos;
using QuizSystem_backend.DTOs.StudentDtos;
using QuizSystem_backend.Models;

namespace QuizSystem_backend.services
{
    public interface ICourseClassService
    {
        Task<IEnumerable<CourseClassDto>> GetCourseClassesAsync(Guid userId, string role);
        Task<CourseClassDto> GetCourseClassByIdAsync(Guid id);
        Task<(bool success, string? message, object? data)> AddStudentToCourseAsync(StudentCourseClassDto scc);
        Task<IEnumerable<StudentDto>> GetStudentByCourseClassAsync(Guid id);
        Task<CourseClassDto> AddCourseClassAsync(CourseClassDto dto);
        Task<CourseClassDto> UpdateCourseClassAsync(Guid id, CourseClassDto dto);
        Task<bool> DeleteCourseClassAsync(Guid id);

        Task<IEnumerable<SubjectDto>> GetSubjectsAsync();
        Task<(bool success, string? message, object? data)> DeleteStudentFromCourseAsync(StudentCourseClassDto scc);
        Task<List<CourseClassSearchDto>> SearchCourseClass(string key, int limit);


    }
}
