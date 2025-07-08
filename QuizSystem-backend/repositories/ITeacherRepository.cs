using QuizSystem_backend.Models;

namespace QuizSystem_backend.repositories
{
    public interface ITeacherRepository
    {
        Task<List<Teacher>> GetAllTeacherAsync();
        Task<bool> SaveTeachersAsync(List<Teacher> teachers);
    }
}
