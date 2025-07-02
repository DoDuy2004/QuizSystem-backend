using Microsoft.EntityFrameworkCore;
using QuizSystem_backend.Models;

namespace QuizSystem_backend.repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly QuizSystemDbContext _context;
        public StudentRepository(QuizSystemDbContext context) 
        {
            _context = context;
        }
        public async Task<IEnumerable<Student>> GetStudentsAsync()
        {
            var students = await _context.Users.OfType<Student>().ToListAsync();

            return students;
        }
        public async Task<Student> GetByIdAsync(Guid id)
        {
            var student = await _context.Users.OfType<Student>().FirstOrDefaultAsync(s => s.Id == id);

            return student!;
        }
    }
}
