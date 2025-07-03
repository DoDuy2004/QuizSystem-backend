using QuizSystem_backend.DTOs.StudentDtos;
using QuizSystem_backend.Models;

namespace QuizSystem_backend.services
{
    public interface IUserService
    {
        Task<AppUser> GetUserByUsernameAsync(string username);
        Task<AppUser> GetUserByIdAsync(Guid userId);
        Task<List<StudentImportDto>> ImportFileStudent(IFormFile file);
    }
}
