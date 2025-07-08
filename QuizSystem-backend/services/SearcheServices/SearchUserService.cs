using Microsoft.EntityFrameworkCore;
using QuizSystem_backend.Enums;
using QuizSystem_backend.Models;

namespace QuizSystem_backend.services.SearcheServices
{
    public class SearchUserService
    {
        private readonly QuizSystemDbContext _dbContext;
        SearchUserService(QuizSystemDbContext dbContext)
        {
            _dbContext=dbContext;
        }
        public async Task<List<User>> SearchUsersAsync(Role role, string? keyword)
        {
            var query = _dbContext.Users.Where(u => u.Role == role);

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(u =>
                    u.FullName.Contains(keyword) ||
                    u.Email.Contains(keyword) ||
                    u.Username.Contains(keyword)
                );
            }

            return await query.ToListAsync();
        }

    }
}
