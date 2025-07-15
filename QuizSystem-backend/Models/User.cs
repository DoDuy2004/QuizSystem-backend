using QuizSystem_backend.Enums;
using System.Data;

namespace QuizSystem_backend.Models
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Username { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public bool Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string AvatarUrl { get; set; } = string.Empty;
        public Status Status { get; set; }
        public string PasswordHash { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public Role Role { get; set; }
        public string? Otp {  get; set; }
        public DateTime? OtpExpireTime { get; set; }
        public virtual ICollection<Question>? Questions { get; set; } = new List<Question>();
        public virtual ICollection<CourseClass>? CourseClasses { get; set; } = null!;
        public virtual ICollection<QuestionBank>? QuestionBanks { get; set; } = null!;

        public virtual ICollection<Exam>? Exams { get; set; } = null!;

        public virtual ICollection<Notification>Notifications { get; set; } = new List<Notification>();
        public virtual ICollection<NotificationMessage> NotificationMessages { get; set; }=new List<NotificationMessage>();
    }
}
