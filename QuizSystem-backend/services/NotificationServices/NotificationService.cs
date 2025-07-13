using Microsoft.EntityFrameworkCore;
using QuizSystem_backend.DTOs;
using QuizSystem_backend.Models;
using QuizSystem_backend.services.NotificationServices;

namespace QuizSystem_backend.services.Notificationervices
{
    public class NotificationService : INotificationService
    {
        private readonly QuizSystemDbContext _context;
        public NotificationService(QuizSystemDbContext context)
        {
            _context = context;
        }

        public async Task AddNotificationAsync(Guid userId, string title, string message)
        {
            var notif = new Notification
            {
                UserId = userId,
                Title = title,
                Message = message
            };
            _context.Notification.Add(notif);
            await _context.SaveChangesAsync();

            var count = await _context.Notification
                .Where(n => n.UserId == userId)
                .CountAsync();

            if (count > 10)
            {
                var oldNotifs = await _context.Notification
                    .Where(n => n.UserId == userId)
                    .OrderBy(n => n.CreatedAt)
                    .Take(1)
                    .ToListAsync();

                _context.Notification.RemoveRange(oldNotifs);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IList<NotificationDto>> GetNotificationAsync(Guid userId, int max = 10)
        {
            return await _context.Notification
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .Take(max)
                .Select(n => new NotificationDto
                {
                    Id = n.Id,
                    Title = n.Title,
                    Message = n.Message,
                    CreatedAt = n.CreatedAt,
                    IsRead = n.IsRead
                })
                .ToListAsync();
        }
        public async Task<int> GetUnreadCountAsync(Guid userId)
        {
            return await _context.Notification
                .Where(n => n.UserId == userId && !n.IsRead)
                .CountAsync();
        }
        public async Task MarkAsReadAsync(Guid notificationId)
        {
            var notif = await _context.Notification.FindAsync(notificationId);
            if (notif == null) throw new KeyNotFoundException("Notification not found");
            notif.IsRead = true;
            await _context.SaveChangesAsync();
        }
        public async Task DeleteNotificationAsync(Guid notificationId)
        {
            var notif = await _context.Notification.FindAsync(notificationId);
            if (notif == null) throw new KeyNotFoundException("Notification not found");
            _context.Notification.Remove(notif);
            await _context.SaveChangesAsync();
        }
    }
}
