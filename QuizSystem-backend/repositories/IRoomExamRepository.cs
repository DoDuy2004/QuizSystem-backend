﻿using QuizSystem_backend.DTOs.StudentDtos;
using QuizSystem_backend.Models;

namespace QuizSystem_backend.repositories
{
    public interface IRoomExamRepository
    {
        Task<RoomExam> AddAsync(RoomExam roomExam);
        Task<IEnumerable<RoomExam>> GetAllAsync();
        Task<RoomExam?> GetByIdAsync(Guid id);
        Task SaveChangesAsync();
        Task<bool> DeleteAsync(Guid id);
        Task<IEnumerable<RoomExam>> GetByRoomIdAsync(Guid roomId);
        Task<bool> UpdateAsync(RoomExam roomExam);
        Task<bool> ExistsAsync(Guid id);
        
        
        //Task<bool> IsStudentInRoomAsync(Guid roomExamId, string Email);
        Task <List<Exam>>GetListExamAsync(int limit, string key);
    }
}
