using Microsoft.EntityFrameworkCore;
using QuizSystem_backend.Models;

namespace QuizSystem_backend.repositories
{
    public class TeacherRepository : ITeacherRepository
    {
        private readonly QuizSystemDbContext _context;
        public TeacherRepository(QuizSystemDbContext context)
        {
            _context = context;
        }
        public async Task<List<Teacher>> GetAllTeacherAsync()
        {
            return await _context.Teachers.ToListAsync();
        }
        public async Task<bool> SaveTeachersAsync(List<Teacher> teachers)
        {

            if (teachers == null || teachers.Count == 0)
            {
                return await Task.FromResult(false);
            }
            try
            {
                await _context.Teachers.AddRangeAsync(teachers);
                return await Task.FromResult(_context.SaveChanges() > 0);
            }
            catch (Exception)
            {
                return await Task.FromResult(false);

            }
        }
    }
}
