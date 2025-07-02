using QuizSystem_backend.Models;

namespace QuizSystem_backend.repositories
{
    public interface IUserRepository
    {
        Task<AppUser> GetByUsernameAsync(string username);
        Task<AppUser> GetByIdAsync(Guid userId);
    }
}
