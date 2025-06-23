using Microsoft.EntityFrameworkCore;
using QuizSystem_backend.Models;

namespace QuizSystem_backend.repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly QuizSystemDbContext _context;

        public UserRepository(QuizSystemDbContext context) 
        {
            _context = context;
        }
        public async Task<User> GetByUsernameAsync(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username || u.Email == username);

            return user!;
        }
        public async Task<User> GetByIdAsync(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);

            return user!;
        }
    }
}
