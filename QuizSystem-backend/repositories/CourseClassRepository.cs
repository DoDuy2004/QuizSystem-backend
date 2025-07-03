using Microsoft.EntityFrameworkCore;
using QuizSystem_backend.Models;

namespace QuizSystem_backend.repositories
{
    public class CourseClassRepository : ICourseClassRepository
    {
        private readonly QuizSystemDbContext _context;
        public CourseClassRepository(QuizSystemDbContext context) 
        { 
            _context = context;
        }
        public async Task<IEnumerable<CourseClass>> GetCourseClassesAsync()
        {
            var coures = await _context.CourseClasses.Include(cc => cc.Teacher).ToListAsync();

            return coures;
        }
        public async Task<IEnumerable<CourseClass>> GetCourseClassesByTeacherAsync(Guid userId)
        {
            var coures = await _context.CourseClasses.Where(cc => cc.TeacherId == userId).ToListAsync();

            return coures!;
        }
        public async Task<CourseClass> GetByIdAsync(Guid id)
        {
            var course = await _context.CourseClasses.Include(cc => cc.Teacher).FirstOrDefaultAsync(cc => cc.Id == id);

            return course!;
        }

        public async Task<Student> AddStudentToCourseAsync(StudentCourseClass scc)
        {
            await _context.StudentCourseClasses.AddAsync(scc);

            await _context.SaveChangesAsync();

            var student = await _context.Users.OfType<Student>().FirstOrDefaultAsync(s => s.Id == scc.StudentId);
            return student!;
        }
        public async Task<IEnumerable<Student>> GetStudentByCourseClassAsync(Guid id)
        {
            var students = await _context.StudentCourseClasses.Include(c => c.Student).Where(scc => scc.CourseClassId == id).Select(scc => scc.Student).ToListAsync();

            return students;
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<CourseClass> AddAsync(CourseClass cc)
        {
            _context.CourseClasses.Add(cc);

            await _context.SaveChangesAsync();

            return cc;
        }
        public async Task<IEnumerable<Subject>> GetSubjectsAsync()
        {
           var subjects = await _context.Subjects.ToListAsync();

            return subjects;
        }
    }
}
