using QuizSystem_backend.Enums;
using QuizSystem_backend.Models;

namespace QuizSystem_backend.DTOs
{
    public class CourseClassDto
    {
        public CourseClassDto() { }
        public CourseClassDto(CourseClass course) 
        {
            var teacher = course.User != null ? new UserDto(course.User) : null;

            Id = course.Id;
            ClassCode = course.ClassCode;
            Name = course.Name;
            Credit = course.Credit;
            Status = course.Status;
            TeacherId = course.UserId;
            SubjectId = course.SubjectId;
            Teacher = teacher;
            Description = course.Description;
        }
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? ClassCode { get; set; } = null!;
        public string Name { get; set; } = null!;

        public int Credit { get; set; } // số tín chỉ
        public Status Status { get; set; }
        public Guid TeacherId { get; set; }
        public Guid SubjectId { get; set; }
        public string? Description { get; set; }

        // Navigation
        public virtual UserDto? Teacher { get; set; } = null!;
        public virtual SubjectDto? Subject { get; set; } = null!;
    }
}
