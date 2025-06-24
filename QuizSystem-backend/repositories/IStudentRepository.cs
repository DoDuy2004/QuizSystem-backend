using QuizSystem_backend.Models;

namespace QuizSystem_backend.repositories
{
    public interface IStudentRepository
    {
        Task<IEnumerable<Student>> GetStudentsAsync();
        Task<Student> GetByIdAsync(Guid id);
    }
}
