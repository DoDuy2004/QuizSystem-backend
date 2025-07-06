using QuizSystem_backend.Enums;

namespace QuizSystem_backend.Models
{
    public class StudentExam // bài thi
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid StudentId { get; set; }
        public Guid? RoomId { get; set; }
        public Guid ExamId { get; set; }
        public int DurationMinutes { get; set; }
        public Status Status { get; set; }
        public float Grade { get; set; }
        public string Note { get; set; } = string.Empty;
        public SubmitStatus SubmitStatus { get; set; } = SubmitStatus.NotSubmitted;
        public virtual Exam Exam { get; set; } = null!;
        public virtual RoomExam Room { get; set; } = null!;
        public virtual ICollection<StudentExamDetail> StudentExamDetails { get; set; } = null!;
    }
}
