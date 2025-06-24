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
            var students = await _context.Students.ToListAsync();

            return students;
        }
        public async Task<Student> GetByIdAsync(Guid id)
        {
            var student = await _context.Students.FindAsync(id);

            return student!;
        }
    }
}
