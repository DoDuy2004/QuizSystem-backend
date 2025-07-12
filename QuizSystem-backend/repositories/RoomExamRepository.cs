using Microsoft.EntityFrameworkCore;
using QuizSystem_backend.DTOs.StudentDtos;
using QuizSystem_backend.Models;

namespace QuizSystem_backend.repositories
{
    public class RoomExamRepository : IRoomExamRepository
    {
        private readonly QuizSystemDbContext _context;
        public RoomExamRepository(QuizSystemDbContext context)
        {
            _context = context;
        }
        public async Task<RoomExam> AddAsync(RoomExam roomExam)
        {
            _context.RoomExams.Add(roomExam);
            await _context.SaveChangesAsync();

            return await _context.RoomExams
                .Include(r => r.Subject)
                .Include(r => r.Course)
                .Include(r => r.Exam)
                .FirstOrDefaultAsync(r => r.Id == roomExam.Id);

        }
        public async Task<IEnumerable<RoomExam>> GetAllAsync()
        {
            return await _context.RoomExams
                .Include(r => r.Subject)
                .Include(r => r.Course)
                .Include(r => r.Exam)
                .ToListAsync();
        }
        public async Task<RoomExam?> GetByIdAsync(Guid id)
        {
            return await _context.RoomExams
                .Include(r => r.Subject)
                .Include(r => r.Course)
                .Include(r => r.Exam)
                .FirstOrDefaultAsync(r => r.Id == id);
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<bool> DeleteAsync(Guid id)
        {
            var roomExam = await GetByIdAsync(id);

            if (roomExam == null) return false;

            _context.RoomExams.Remove(roomExam);

            await SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<RoomExam>> GetByRoomIdAsync(Guid roomId)
        {
            return await _context.RoomExams
                .Where(re => re.Id == roomId)
                .ToListAsync();

        }
        public async Task<bool> UpdateAsync(RoomExam roomExam)
        {
            _context.RoomExams.Update(roomExam);
            await SaveChangesAsync();
            return true;
        }
        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.RoomExams.AnyAsync(re => re.Id == id);
        }

        public Task<List<Exam>> GetListExamAsync(int limit, string key)
        {
            throw new NotImplementedException();
        }

        //public async Task<bool> IsStudentInRoomAsync(Guid roomExamId,string Email)
        //{
        //    var roomExam = await _context.RoomExams
        //        .Include(re => re.Course)
        //            .ThenInclude(c => c.Students)
        //        .FirstOrDefaultAsync(re => re.Id == roomExamId)
        //        .;
        //    if (roomExam == null) return false;
        //    return roomExam.StudentExams.Any(se => se.Student.Email == Email);
        //}



    }
}