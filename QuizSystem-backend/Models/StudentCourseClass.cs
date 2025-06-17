using QuizSystem_backend.Enums;

namespace QuizSystem_backend.Models
{
    public class StudentCourseClass
    {
        public Guid StudentId { get; set; }
        public Guid CourseClass { get; set; }

        public float? Grade { get; set; }

        public string? note { get; set; } = string.Empty;

        public Status Status { get; set; }

        public virtual Student Student { get; set; } = null!;
        public virtual CourseClass Course { get; set; } = null!;
    }
}
