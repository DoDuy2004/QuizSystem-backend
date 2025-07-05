using QuizSystem_backend.DTOs;
using QuizSystem_backend.DTOs.StudentDtos;
using QuizSystem_backend.Models;

namespace QuizSystem_backend.services
{
    public interface IRoomExamService
    {
        Task<RoomExamDto> AddRoomExamAsync(RoomExamDto roomExamDto);
        Task<IEnumerable<RoomExamDto>> GetAllRoomExamsAsync();
        Task<RoomExamDto> GetRoomExamByIdAsync(Guid id);
        Task SaveChangesAsync();
        Task<bool> DeleteRoomExamAsync(Guid id);
        
        Task<bool> UpdateRoomExamAsync(RoomExamDto roomExamDto);
        Task<List<StudentImportDto>> ImportStudenInRoomExam(IFormFile file, Guid roomExamId);


    }
}
