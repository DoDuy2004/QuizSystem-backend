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
                Id = Guid.NewGuid(),
                Title = title,
                Message = message,
                CreatedAt = DateTime.UtcNow
            };
            _context.Notification.Add(notif);
            var userNotification=new UserNotification
            {
                UserId = userId,
                NotificationId = notif.Id,
                IsRead = false,
            };
            _context.UserNotifications.Add(userNotification);

           
            await _context.SaveChangesAsync();

            var count = await _context.UserNotifications
                .Where(un => un.UserId == userId)
                .CountAsync();

            if (count > 10)
            {
                var oldNotifs = await _context.UserNotifications.Include(un=>un.Notification)
                    .Where(un => un.UserId == userId)
                    .OrderBy(un => un.Notification.CreatedAt)
                    .Take(1)
                    .ToListAsync();

                _context.UserNotifications.RemoveRange(oldNotifs);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IList<NotificationDto>> GetNotificationAsync(Guid userId,int max = 10)
        {
            return await _context.UserNotifications
                .Where(un => un.UserId == userId).Include(un => un.Notification)
                .OrderByDescending(un => un.Notification.CreatedAt)
                .Take(max)
                .Select(un => new NotificationDto
                {
                    Id = un.NotificationId,
                    Title = un.Notification.Title,
                    Message = un.Notification.Message,
                    CreatedAt = un.Notification.CreatedAt,
                    IsRead = un.IsRead
                })
                .ToListAsync();
        }
        public async Task<int> GetUnreadCountAsync(Guid userId)
        {
            return await _context.UserNotifications
                .Where(un => un.UserId == userId && !un.IsRead)
                .CountAsync();
        }
        public async Task MarkAsReadAsync(Guid userId,List<NotificationDto> listNotificationDto)
        {
            foreach(var notiDto in listNotificationDto)
            {
                var notif = await _context.UserNotifications
                    .FirstOrDefaultAsync(un => un.UserId == userId && un.NotificationId == notiDto.Id);
                if (notif == null) throw new KeyNotFoundException("Notification not found");
                notif.IsRead = true;
            }
            
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
