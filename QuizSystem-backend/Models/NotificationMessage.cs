namespace QuizSystem_backend.Models
{
    public class NotificationMessage
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Content { get; set; } = string.Empty;
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;
        public Guid NotificationId { get; set; }
        public Guid UserId { get; set; }

        //Navigation
        public NotificationForCourseClass Notification { get; set; } = null!;
        public User User { get; set; }=null!;
    }
}
