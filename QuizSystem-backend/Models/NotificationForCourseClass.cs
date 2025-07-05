using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizSystem_backend.Models
{
    public class NotificationForCourseClass
    {
        [Key]
        Guid Id { get; set; } = Guid.NewGuid();
        string Content { get; set; } = string.Empty;
        Guid CourseClassId { get; set; }
        Guid TeacherId { get; set; }
        DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        User User { get; set; } = null!;
        // Navigation
        [ForeignKey("TeacherId")]
        public virtual Teacher Teacher { get; set; } = null!;
        [ForeignKey("CourseClassId")]
        CourseClass CourseClass { get; set; } = null!;
    }
}
