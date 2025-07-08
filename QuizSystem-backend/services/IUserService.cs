using Microsoft.AspNetCore.Identity;
using QuizSystem_backend.DTOs.UserDtos;
using QuizSystem_backend.Models;

namespace QuizSystem_backend.services
{
    public interface IUserService
    {
        Task<User> GetUserByUsernameAsync(string username);
        Task<User> GetUserByIdAsync(Guid userId);
        Task<bool> ChangePasswordAsync(Guid userId, string oldPassword, string newPassword);
        Task<(bool Succeed, string Message)> AddUser(AddUserDtos userDto);
    }
}
