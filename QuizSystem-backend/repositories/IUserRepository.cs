using Microsoft.AspNetCore.Identity;
using QuizSystem_backend.Models;

namespace QuizSystem_backend.repositories
{
    public interface IUserRepository
    {
        Task<User> GetByUsernameAsync(string username);
        Task<User> GetByIdAsync(Guid userId);
        bool CheckPasswordAsync(User user, string password);
        Task<bool> ChangePasswordAsync(User user, string newPassword);
        
        Task AddSingle<T>(T user) where T : User;
        
    }
}
