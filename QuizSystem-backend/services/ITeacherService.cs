using QuizSystem_backend.DTOs.TeacherDtos;
using QuizSystem_backend.Models;

namespace QuizSystem_backend.services
{
    public interface ITeacherService
    {
        Task<List<Teacher>> ImportTeacherConfirm(List<TeacherImportDto> teachersPreview);
        Task<List<TeacherImportDto>> ImportFileTeacherPreview(IFormFile file);
        Task<NotificationForCourseClass> AddNotificationAsync(NotificationForCourseClass notification);
    }
}
