using QuizSystem_backend.Models;

namespace QuizSystem_backend.services
{
    public interface IStudentService
    {
        Task<IEnumerable<Student>> GetStudentsAsync();
        Task<Student> GetStudentByIdAsync(Guid id);
    }
}
