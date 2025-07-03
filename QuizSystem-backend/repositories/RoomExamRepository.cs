using Microsoft.EntityFrameworkCore;
using QuizSystem_backend.Models;

namespace QuizSystem_backend.repositories
{
    public class RoomExamRepository:IRoomExamRepository
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
            return roomExam;
        }
        public async Task<IEnumerable<RoomExam>> GetAllAsync()
        {
            return await _context.RoomExams
                .Include(re => re.Exams)
                .Include(re => re.StudentExams)
                .ToListAsync();
        }
        public async Task<RoomExam?> GetByIdAsync(Guid id)
        {
            return await _context.RoomExams.FindAsync(id);
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
       
        
    }
}
