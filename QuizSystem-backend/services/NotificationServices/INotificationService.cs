using QuizSystem_backend.DTOs;

namespace QuizSystem_backend.services.NotificationServices
{
    public interface INotificationService
    {
        Task AddNotificationAsync(Guid userId, string title, string message);
        Task<IList<NotificationDto>> GetNotificationAsync(Guid userId, int max = 10);
        Task MarkAsReadAsync(Guid notificationId);
        Task DeleteNotificationAsync(Guid notificationId);
        Task<int> GetUnreadCountAsync(Guid userId);

    }
}
