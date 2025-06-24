using QuizSystem_backend.Enums;
using QuizSystem_backend.Models;

namespace QuizSystem_backend.DTOs
{
    public class StudentCourseClassDto
    {
        public Guid StudentId { get; set; }
        public Guid CourseClassId { get; set; }

        public float? Grade { get; set; } = null;

        public string? Note { get; set; } = string.Empty;

        public Status Status { get; set; } = Status.ACTIVE;

    }
}
