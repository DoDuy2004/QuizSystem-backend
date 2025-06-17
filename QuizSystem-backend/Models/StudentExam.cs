using QuizSystem_backend.Enums;

namespace QuizSystem_backend.Models
{
    public class StudentExam // bài thi
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid StudentId { get; set; }
        public Guid CourseClass { get; set; }
        public Guid ExamId { get; set; }
        public int DurationMinutes { get; set; }
        public Status Status { get; set; }

        public virtual StudentCourseClass Student { get; set; } = null!;
        public virtual Exam Exam { get; set; } = null!;

        public ICollection<StudentExamDetail> StudentExamDetails { get; set; } = null!;
    }
}
