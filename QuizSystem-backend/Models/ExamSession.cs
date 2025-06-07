namespace QuizSystem_backend.Models
{
    public class ExamSession
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // Tên kỳ thi: "Kỳ thi Giữa kỳ", "Kỳ thi Cuối kỳ", ...
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int Status { get; set; }

        // Navigation
        public virtual ICollection<Exam> Exams { get; set; } = new List<Exam>();
        public ICollection<ExamSessionSubject> ExamSessionSubjects { get; set; } = new List<ExamSessionSubject>();
    }
}
