using QuizSystem_backend.Enums;
using QuizSystem_backend.Models;

namespace QuizSystem_backend.repositories
{
    public interface IStudentRepository
    {
        Task<IEnumerable<Student>> GetStudentsAsync();
        Task<Student> GetByIdAsync(Guid id);
        Task<bool> SaveStudentsAsync(List<Student> students);
        Task SetStatusAsync(Guid roomId, Guid studentId, SubmitStatus status);
    }
}
