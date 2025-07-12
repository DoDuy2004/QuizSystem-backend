using QuizSystem_backend.Enums;

namespace QuizSystem_backend.Models
{
    public class Exam // đề thi
    {
        
        public Guid Id { get; set; } = Guid.NewGuid();
        public string ExamCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int DurationMinutes { get; set; }
        public int NoOfQuestions { get; set; }
        //public float TotalScore { get; set; }
        public Status Status { get; set; }
        public Guid SubjectId { get; set; }
        public Guid UserId { get; set; }

        // Navigation
        public virtual ICollection<RoomExam> RoomExams { get; set; } = null!;

        public virtual ICollection<ExamQuestion> ExamQuestions { get; set; } = null!;
        public virtual User? User { get; set; } = null!;
        public virtual Subject Subject { get; set; } = null!;
        public virtual ICollection<StudentExam> StudentExams { get; set;} = null!;
    }
}
