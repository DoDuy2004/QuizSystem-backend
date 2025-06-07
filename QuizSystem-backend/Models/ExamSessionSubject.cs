namespace QuizSystem_backend.Models
{
    public class ExamSessionSubject
    {
        public int ExamSessionId { get; set; }
        public virtual ExamSession ExamSession { get; set; } = null!;

        public int SubjectId { get; set; }
        public virtual Subject Subject { get; set; } = null!;

        public DateTime ExamDate { get; set; } // Ngày tháng thi của môn học
    }
}
