﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
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

        public bool CheckPasswordAsync(User user, string password)
        {
            var hasher = new PasswordHasher<User>();
            var result = hasher.VerifyHashedPassword(user, user.PasswordHash, password);
            return result == PasswordVerificationResult.Success;
        }
        
        public async Task<bool> ChangePasswordAsync(User user, string newPassword)
        {
            var hasher = new PasswordHasher<User>();
            user.PasswordHash = hasher.HashPassword(user, newPassword);

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }
        

        public async Task AddSingle<T>(T user) where T : User
        {
           
            if(user is Student student)
            {
                await _context.Students.AddAsync(student);
            }
            else if (user is Teacher teacher)
            {
                await _context.Teachers.AddAsync(teacher);
            }
            _context.SaveChanges();
        }

        


    }
}
