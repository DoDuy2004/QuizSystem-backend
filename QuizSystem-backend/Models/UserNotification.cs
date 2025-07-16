namespace QuizSystem_backend.Models
{
    public class UserNotification
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public Guid NotificationId { get; set; }
        public Notification Notification { get; set; } = null!;
        public bool IsRead { get; set; } = false;

    }
}
