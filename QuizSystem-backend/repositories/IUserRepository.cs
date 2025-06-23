using QuizSystem_backend.Models;

namespace QuizSystem_backend.repositories
{
    public interface IUserRepository
    {
        Task<User> GetByUsernameAsync(string username);
        Task<User> GetByIdAsync(Guid userId);
    }
}
