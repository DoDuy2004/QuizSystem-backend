using QuizSystem_backend.Models;

namespace QuizSystem_backend.services
{
    public interface IUserService
    {
        Task<User> GetUserByUsernameAsync(string username);
        Task<User> GetUserByIdAsync(Guid userId);
    }
}
