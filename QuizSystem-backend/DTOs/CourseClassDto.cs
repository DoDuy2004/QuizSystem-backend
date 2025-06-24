using QuizSystem_backend.Enums;
using QuizSystem_backend.Models;

namespace QuizSystem_backend.DTOs
{
    public class CourseClassDto
    {
        public CourseClassDto() { }
        public CourseClassDto(CourseClass course) 
        {
            Id = course.Id;
            ClassCode = course.ClassCode;
            Name = course.Name;
            Credit = course.Credit;
            Status = course.Status;
            TeacherId = course.TeacherId;
            Subject = course.Subject;
        }
        public Guid Id { get; set; } = Guid.NewGuid();
        public string ClassCode { get; set; } = null!;
        public string Name { get; set; } = null!;

        public int Credit { get; set; } // số tín chỉ
        public Status Status { get; set; }
        public Guid TeacherId { get; set; }
        public string Subject { get; set; } = null!;

        // Navigation
        public virtual TeacherDto? Teacher { get; set; } = null!;
        //public virtual Subject Subject { get; set; } = null!;
    }
}
