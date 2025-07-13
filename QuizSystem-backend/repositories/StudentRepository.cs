using Microsoft.EntityFrameworkCore;
using QuizSystem_backend.Enums;
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

        public Task<bool> SaveStudentsAsync(List<Student> students)
        {
            if (students == null || students.Count == 0)
            {
                return Task.FromResult(false);
            }
            try
            {
                _context.Students.AddRange(students);
                return Task.FromResult(_context.SaveChanges() > 0);
            }
            catch (Exception)
            {
                // Log the exception if needed
                return Task.FromResult(false);
            }
        }
        public async Task SetStatusAsync(Guid roomId, Guid studentId, SubmitStatus status)
        {
            // Giả sử có entity StudentRoomStatus lưu status từng SV
            var record = await _context.StudentRoomExams
                .FirstOrDefaultAsync(r => r.RoomExamId == roomId
                                       && r.StudentId == studentId);

            if (record == null)
            {
                // Nếu chưa có, khởi tạo mới
                record = new StudentRoomExam
                {
                    RoomExamId = roomId,
                    StudentId = studentId,
                    SubmitStatus = status,
                    UpdateAt = DateTime.UtcNow
                };
                _context.StudentRoomExams.Add(record);
            }
            else
            {
                record.SubmitStatus = status;
                record.UpdateAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
        }


    }
}
