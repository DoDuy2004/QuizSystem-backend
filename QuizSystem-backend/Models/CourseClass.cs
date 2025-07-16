using QuizSystem_backend.DTOs;
using QuizSystem_backend.Enums;

namespace QuizSystem_backend.Models
{
    public class CourseClass
    {
        public CourseClass() {  }
        public CourseClass(CourseClassDto dto)
        {
            Id = dto.Id;
            ClassCode = dto.ClassCode;
            Name = dto.Name;
            Credit = dto.Credit;
            Status = dto.Status;
            UserId = dto.TeacherId;
            SubjectId = dto.SubjectId;
            User = this.User;
            Description = dto.Description;
        }
        public Guid Id { get; set; } = Guid.NewGuid();
        public string ClassCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        public int Credit { get; set; } // số tín chỉ
        public Status Status { get; set; }
        public string Description { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public Guid SubjectId { get; set; }

        // Navigation
        public virtual User User { get; set; } = null!;
        public virtual Subject Subject { get; set; } = null!;
        public virtual ICollection<Chapter> Chapters { get; set; } = null!;
        public virtual ICollection<RoomExam> RoomExams { get; set; } = null;
        public ICollection<StudentCourseClass> Students { get; set; } = null!;
        public virtual ICollection<NotFiniteNumberException> Notifications { get; set; } = new List<NotFiniteNumberException>();
        public ICollection<NotificationForCourseClass> NotificationForCourseClasses { get; set;} = new List<NotificationForCourseClass>();


    }
}
