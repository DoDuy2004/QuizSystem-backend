using QuizSystem_backend.DTOs;
using QuizSystem_backend.Enums;

namespace QuizSystem_backend.Models
{
    public class StudentCourseClass
    {
        public StudentCourseClass() { }
        public StudentCourseClass(StudentCourseClassDto dto) 
        {
            StudentId = dto.StudentId;
            CourseClassId = dto.CourseClassId;
            Grade = dto.Grade;
            Note = dto.Note;
            Status = dto.Status;
        }
        public Guid StudentId { get; set; }
        public Guid CourseClassId { get; set; }

        public float? Grade { get; set; }

        public string? Note { get; set; } = string.Empty;

        public Status Status { get; set; }

        public virtual Student Student { get; set; } = null!;
        public virtual CourseClass Course { get; set; } = null!;
    }
}
