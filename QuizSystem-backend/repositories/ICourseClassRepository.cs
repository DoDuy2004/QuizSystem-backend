using QuizSystem_backend.Models;

namespace QuizSystem_backend.repositories
{
    public interface ICourseClassRepository
    {
        Task<IEnumerable<CourseClass>> GetCourseClassesAsync();
        Task<CourseClass> GetByIdAsync(Guid id);
        Task<CourseClass> AddAsync(CourseClass cc);
        Task SaveChangesAsync();
        Task<Student> AddStudentToCourseAsync(StudentCourseClass scc);
        Task<IEnumerable<Student>> GetStudentByCourseClassAsync(Guid id);
        Task<IEnumerable<string>> GetSubjectsAsync();
    }
}
