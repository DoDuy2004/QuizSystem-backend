namespace QuizSystem_backend.Models
{
    public class Exam
    {
        public int Id { get; set; }
        public string ExamCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public int DurationMinutes { get; set; }
        public int NumberOfQuestions { get; set; }
        public double TotalScore { get; set; }
        public int SubjectId { get; set; }
        public int Status { get; set; }
        public int ExamSessionId { get; set; }

        // Navigation
        public virtual Subject Subject { get; set; } = null!;
        public virtual ExamSession ExamSession { get; set; } = null!;

        public virtual ICollection<ExamQuestion> ExamQuestions { get; set; } = new List<ExamQuestion>();
    }
}
